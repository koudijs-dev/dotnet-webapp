# .NET web app with Valkey-backed state

This example keeps three counters in Valkey instead of process memory, so the values survive app restarts and pod replacement.

## Local dev demo

Run the app and a single persistent Valkey container with:

```powershell
docker compose up
```

Then open:

* `http://localhost:8080`

The `webapp` service runs `dotnet run` inside Compose for a more stable local demo.
The Prometheus metrics endpoint is exposed at `http://localhost:8080/metrics`.

## What the demo shows

* Three counters are stored in Valkey.
* Restarting the web app does not reset them.
* The app replicas in Kubernetes can be replaced without losing the values, as long as they point at the same Valkey deployment.

## Container image

To build the app image directly:

```powershell
docker build . -t simple-container
docker run --rm -p 9006:80 simple-container
```

## Kubernetes manifests

For the workshop Lab 5 path, use the prepared Kustomize bundle:

```bash
kubectl apply -k k8s/lab5
```

That bundle creates:

* a `lab5` namespace;
* a `ValkeyCluster` named `cache`;
* a stable `valkey-seed` Service for the app to connect to;
* the .NET web app Deployment + Service;
* a Traefik `IngressRoute` at `https://lab5.k8s.koudijs.dev`;
* a `ServiceMonitor` for the app's `/metrics` endpoint.

The deployment and Helm chart expose these environment variables:

* `Redis__Configuration`
* `Redis__InstanceName`

Set `Redis__Configuration` to a reachable Valkey endpoint in your environment.

The raw Kubernetes manifests also include a basic `ServiceMonitor` for Prometheus Operator users.
