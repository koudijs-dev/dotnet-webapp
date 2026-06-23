using System.Text;
using System.Text.Json;

namespace simple_container.Services;

public static class RequestUserContextReader
{
    private static readonly string[] EmailHeaders =
    [
        "X-Auth-Request-Email",
        "X-Forwarded-Email",
        "X-Email",
        "X-MS-CLIENT-PRINCIPAL-NAME"
    ];

    private static readonly string[] GroupHeaders =
    [
        "X-Auth-Request-Groups",
        "X-Forwarded-Groups",
        "X-Groups",
        "X-MS-CLIENT-PRINCIPAL-GROUPS"
    ];

    private static readonly string[] IpHeaders =
    [
        "X-Forwarded-For",
        "X-Real-IP"
    ];

    public static RequestUserContext Read(HttpContext httpContext)
    {
        var emailAddress = FirstNonEmptyHeader(httpContext, EmailHeaders);
        var firstGroup = ParseFirstGroup(FirstNonEmptyHeader(httpContext, GroupHeaders));
        var ipAddress = ParseIpAddress(httpContext);

        if (TryReadAzureClientPrincipal(httpContext, out var principalEmail, out var principalGroup))
        {
            emailAddress ??= principalEmail;
            firstGroup ??= principalGroup;
        }

        return new RequestUserContext(emailAddress, firstGroup, ipAddress);
    }

    private static string? FirstNonEmptyHeader(HttpContext httpContext, params string[] headerNames)
    {
        foreach (var headerName in headerNames)
        {
            if (!httpContext.Request.Headers.TryGetValue(headerName, out var headerValue))
            {
                continue;
            }

            var value = string.Join(",", headerValue.ToArray()).Trim();
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return null;
    }

    private static string? ParseIpAddress(HttpContext httpContext)
    {
        var forwardedFor = FirstNonEmptyHeader(httpContext, IpHeaders);
        if (!string.IsNullOrWhiteSpace(forwardedFor))
        {
            return forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault();
        }

        return httpContext.Connection.RemoteIpAddress?.ToString();
    }

    private static string? ParseFirstGroup(string? groupValue)
    {
        if (string.IsNullOrWhiteSpace(groupValue))
        {
            return null;
        }

        var trimmed = groupValue.Trim();

        if (trimmed.StartsWith("[", StringComparison.Ordinal))
        {
            try
            {
                var groups = JsonSerializer.Deserialize<string[]>(trimmed);
                return groups?.FirstOrDefault(group => !string.IsNullOrWhiteSpace(group));
            }
            catch (JsonException)
            {
            }
        }

        return trimmed
            .Split([',', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .FirstOrDefault();
    }

    private static bool TryReadAzureClientPrincipal(HttpContext httpContext, out string? emailAddress, out string? firstGroup)
    {
        emailAddress = null;
        firstGroup = null;

        if (!httpContext.Request.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL", out var principalValues))
        {
            return false;
        }

        var encodedPrincipal = principalValues.ToString();
        if (string.IsNullOrWhiteSpace(encodedPrincipal))
        {
            return false;
        }

        try
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(PadBase64(encodedPrincipal)));
            var principal = JsonSerializer.Deserialize<ClientPrincipalPayload>(json);

            emailAddress = principal?.UserDetails;

            var claims = principal?.Claims ?? [];
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                emailAddress = claims
                    .FirstOrDefault(claim => EmailClaimTypes.Contains(claim.Typ, StringComparer.OrdinalIgnoreCase))
                    ?.Val;
            }

            firstGroup = claims
                .Where(claim => GroupClaimTypes.Contains(claim.Typ, StringComparer.OrdinalIgnoreCase))
                .Select(claim => claim.Val)
                .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    private static string PadBase64(string value)
    {
        var normalized = value.Replace('-', '+').Replace('_', '/');
        var remainder = normalized.Length % 4;

        return remainder == 0 ? normalized : normalized.PadRight(normalized.Length + (4 - remainder), '=');
    }

    private static readonly string[] EmailClaimTypes =
    [
        "email",
        "emails",
        "preferred_username",
        "upn",
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
    ];

    private static readonly string[] GroupClaimTypes =
    [
        "groups",
        "roles",
        "role",
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ];

    private sealed class ClientPrincipalPayload
    {
        public string? UserDetails { get; set; }
        public ClientPrincipalClaim[]? Claims { get; set; }
    }

    private sealed class ClientPrincipalClaim
    {
        public string? Typ { get; set; }
        public string? Val { get; set; }
    }
}
