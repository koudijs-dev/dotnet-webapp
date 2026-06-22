# .NET web app with Valkey-backed state

This example now keeps three counters in Valkey instead of process memory, so the values survive app restarts and pod replacement.

## Local dev demo

Run the app and a six-node Valkey cluster with:

```powershell
docker compose up
```

Then open:

* `http://localhost:8080`

The `webapp` service runs `dotnet watch`, so edits under `/src` are picked up automatically.

## Why the app runs in Compose too

Valkey Cluster does not behave well behind arbitrary Docker port remapping. For local dev, the simplest reliable setup is to keep the app and cluster on the same Docker network so the client can follow cluster redirects cleanly.

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

The deployment and Helm chart now expose these environment variables:

* `Redis__Configuration`
* `Redis__InstanceName`

Set `Redis__Configuration` to a reachable Valkey node or cluster seed endpoint in your environment.
