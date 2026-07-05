# .NET Version Upgrade — LTCSharp

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0

## Source Control
- **Source Branch**: net10
- **Working Branch**: net10
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)

## Strategy
**Selected**: Bottom-Up (Dependency-First)
**Rationale**: 4 .NET projects crossing the .NET Framework → modern .NET boundary; 2-tier dependency graph with C++/CLI at Tier 1 and 3 C# projects at Tier 2.

### Execution Constraints
- Strict tier ordering: Tier 1 (LTCSharp C++/CLI) must build and validate before Tier 2 begins
- SDK-style conversion is a separate task from TFM upgrade — never merged
- After Tier 1 upgrades to net10.0, Tier 2 C# projects can no longer build on net48 — proceed immediately to Tier 2 task
- Per-project package management during migration; CPM deferred to post-migration
- Unsupported VVVV packages in ExampleDecode: defer resolution with stubs; resolve inline if packages are simply unused
- WinForms API breaking changes: fix inline in the Tier 2 task
- Nullable reference types: leave disabled throughout; enable separately post-migration

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: Bottom-Up

### Project Structure
- Project Approach: In-place
- Package Management: Per-Project (defer CPM to post-migration)

### Compatibility
- Unsupported Packages: Defer Resolution (4 incompatible VVVV packages in ExampleDecode)
- Unsupported API Handling: Fix Inline

### Modernization
- Nullable Reference Types: Leave Disabled
