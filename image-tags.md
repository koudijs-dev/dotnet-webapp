# Container image tags

Two workflows publish images, with a clean split of responsibility:

| Workflow | Trigger | Publishes |
| --- | --- | --- |
| `pipeline.yml` | branch push, PR | prerelease images (never on `main`) |
| `release-please.yml` | accepted release on `main` | official release images + `latest` |

## Prerelease images (branch / PR builds)

The version is the **next** release — a patch bump of the latest `vX.Y.Z` tag — with a
semver prerelease suffix. Each build publishes three tags plus a full-semver OCI label:

| Tag | Mutability | Use |
| --- | --- | --- |
| `0.2.1-pr23` | floating, always overwritten | latest build of this PR |
| `0.2.1-pr23.2` | replaced on amend, new on each added commit | latest build at this commit count |
| `sha-1234567` | immutable (per commit) | this exact commit, forever |

The `org.opencontainers.image.version` label carries the full semver including build
metadata, e.g. `0.2.1-pr23.2+sha-1234567`. Docker tags can't contain `+`, so the build
metadata lives only in the label.

The counter comes from the PR's commit count, so:

* adding a commit bumps the counter (`.2` -> `.3`) and creates a new tag;
* amending or force-pushing keeps the counter and **replaces** the `0.2.1-pr23.2` and
  `0.2.1-pr23` tags with the rebuilt image;
* every distinct commit always keeps its own immutable `sha-*` tag.

Ordering nests cleanly: `0.2.1-pr23` < `0.2.1-pr23.1` < `0.2.1-pr23.2` < ... < `0.2.1`
(the eventual release).

Branch builds without a PR use `branch-<slug>` in place of `pr<n>`, and the workflow run
number as the counter.
