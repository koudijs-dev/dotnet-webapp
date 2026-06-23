FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /app
COPY ./src/*.csproj ./
RUN dotnet restore simple-container.csproj
COPY ./src .
RUN dotnet publish simple-container.csproj --configuration Release --no-restore -o /app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
EXPOSE 80
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "/app/simple-container.dll" ]
