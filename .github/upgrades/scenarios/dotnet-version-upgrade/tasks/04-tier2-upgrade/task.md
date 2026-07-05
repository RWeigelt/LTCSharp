# 04-tier2-upgrade: Upgrade Tier 2 projects to .NET 10

Upgrade all three Tier 2 projects to .NET 10. This task covers TFM changes, package cleanup, incompatible package stubs, and WinForms API fixes. Because all three projects depend on LTCSharp (now net10.0), they cannot build on net48 — proceed with all three immediately after Tier 1 validates.

**LTCNodes\LTCNodes.csproj** → `net10.0`. Remove the five framework-redundant packages now included in the .NET 10 runtime: `Microsoft.Win32.Registry`, `System.Buffers`, `System.Memory`, `System.Numerics.Vectors`, `System.Security.Principal.Windows`. VVVV packages (`VVVV.Core`, `VVVV.PluginInterfaces`, `VVVV.SlimDX`, `VVVV.System.ComponentModel.Composition.Codeplex`, `VVVV.Utils`) are compatible with net10.0 — retain at current versions.

**ExampleEncode\ExampleEncode.csproj** → `net10.0`. Same five framework-redundant packages removed. VVVV packages compatible — retain. No API breaking changes flagged.

**ExampleDecode\ExampleDecode.csproj** → `net10.0-windows` (WinForms requires Windows TFM). This is the most complex project: (1) the same five framework-redundant packages must be removed; (2) four incompatible VVVV packages (`VVVV.Core`, `VVVV.PluginInterfaces`, `VVVV.System.ComponentModel.Composition.Codeplex`, `VVVV.Utils`) must be removed — first check whether ExampleDecode's code actually calls any VVVV APIs; if not, remove directly; if yes, generate minimal type stubs and create a stub-resolution subtask; (3) fix the 7 binary-incompatible WinForms API calls inline — these are typically mechanical changes (event handler signature adjustments, property renames) documented in the .NET breaking changes reference.

Assessment reference for ExampleDecode API issues: `assessment.md`, project scope `ExampleDecode\ExampleDecode.csproj`, category `Api.0001`.

**Done when**: All three projects build targeting .NET 10 with no errors; framework-redundant package references removed; ExampleDecode either has no VVVV API calls (packages removed cleanly) or has compilable stubs with `// TODO: resolve VVVV stub` markers; all 7 WinForms API breaking changes resolved inline; solution builds cleanly.
