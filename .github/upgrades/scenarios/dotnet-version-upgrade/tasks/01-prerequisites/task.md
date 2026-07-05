# 01-prerequisites: Verify upgrade prerequisites

Confirm the environment is ready for the upgrade before any project changes are made. The .NET 10 SDK has already been validated as present. This task verifies the remaining prerequisites: `global.json` compatibility, absence of blocking SDK pins, and confirmation that the `libltc` native C++ project (Tier 0) requires no .NET framework changes whatsoever.

Review the solution's `.github/upgrades/scenarios/dotnet-version-upgrade/assessment.md` for any issues not captured in the plan. Check whether any CI/build scripts reference `net48` explicitly and would need updating.

**Done when**: .NET 10 SDK confirmed compatible with any `global.json` present; `libltc` confirmed as a native C++ project with no TFM change needed; no blocking environment issues identified.
