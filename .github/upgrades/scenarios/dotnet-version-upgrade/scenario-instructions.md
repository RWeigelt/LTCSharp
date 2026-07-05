# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0
- **Scope**: ExampleDecode project only — C++ projects (LTCSharp, libltc) are excluded

## Upgrade Options
- **Upgrade Strategy**: All-at-Once
- **Project Approach**: In-place
- **Nullable Reference Types**: Leave Disabled

## Strategy
**Selected**: All-at-Once
**Rationale**: Single .NET Framework project (ExampleDecode, net48) — no dependency graph to manage.

### Execution Constraints
- Single atomic upgrade — SDK-style conversion and TFM upgrade are separate tasks (never merged)
- SDK conversion stays on net48; TFM change happens in the following task
- packages.config → PackageReference migration is part of the SDK-style conversion task
- Target TFM must be `net10.0-windows` (not `net10.0`) — Windows Forms requires the `-windows` suffix
- C++ projects (LTCSharp, libltc) must not be modified
- Validate full solution build (including C++ projects) after upgrade

## Source Control
- **Source Branch**: net10_2
- **Working Branch**: dotnet-version-upgrade
- **Commit Strategy**: Single Commit at End
- **Branch Sync**: Auto (Merge)

## User Preferences
### Technical Preferences
- **Nullable Reference Types**: Leave Disabled (user preference)
