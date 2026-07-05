# 01-prerequisites: Verify upgrade prerequisites

Confirm the .NET 10 SDK is installed and compatible before any project changes are made.
Check whether a `global.json` file exists in the repository root or any parent directory
that could pin the SDK version to an incompatible range and prevent the upgraded project
from building.

**Done when**: .NET 10 SDK confirmed installed and any `global.json` constraints are verified
to be compatible with net10.0 (or updated if needed).
