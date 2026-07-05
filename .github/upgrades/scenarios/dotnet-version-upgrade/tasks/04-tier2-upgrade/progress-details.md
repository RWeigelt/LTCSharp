# Progress Details — 04-tier2-upgrade

## Summary
All three Tier 2 C# projects upgraded to .NET 10 (`net10.0-windows`). Framework-redundant packages removed, VVVV packages removed from ExampleDecode and ExampleEncode (no VVVV APIs used). All three projects build successfully under .NET 10.

## TFM Changes

| Project | Before | After | Reason for `-windows` |
|---------|--------|-------|----------------------|
| LTCNodes | `net48` | `net10.0-windows` | `NAudio.WinForms` requires Windows platform |
| ExampleEncode | `net48` | `net10.0-windows` | `NAudio.WinForms` transitive requires Windows platform |
| ExampleDecode | `net48` | `net10.0-windows` | WinForms (`UseWindowsForms=true`) |

## Package Changes

### Packages removed from all three projects (now built into .NET 10)
- `Microsoft.Win32.Registry` — part of BCL since .NET Core 3.0
- `System.Buffers` — part of BCL since .NET Core 3.0
- `System.Memory` — part of BCL since .NET Core 3.0
- `System.Numerics.Vectors` — part of BCL since .NET Core 3.0
- `System.Runtime.CompilerServices.Unsafe` — part of BCL since .NET 6
- `System.Security.Principal.Windows` — part of BCL since .NET Core 3.0

### VVVV packages removed from ExampleDecode and ExampleEncode
Both projects had VVVV packages added during SDK conversion (inherited from original packages.config) but **use zero VVVV APIs** — all source files were verified to contain no `VVVV` namespace or API calls. Packages removed cleanly:
- `VVVV.Core` 39.0.0
- `VVVV.PluginInterfaces` 39.0.0
- `VVVV.SlimDX` 1.0.2
- `VVVV.System.ComponentModel.Composition.Codeplex` 2.5.0
- `VVVV.Utils` 39.0.0

### VVVV packages retained in LTCNodes
LTCNodes is a VVVV plugin — all VVVV packages retained at current versions. NU1701 warnings are expected: these packages are .NET Framework-only and restore via compatibility fallback. This is the correct outcome per the upgrade strategy (defer VVVV compatibility resolution to post-migration).

### Legacy `<Reference>` items removed
`System.Data.DataSetExtensions`, `Microsoft.CSharp`, and local VVVV HintPath references (`VVVV.Hosting`, `VVVV.Utils3rdParty`, `VVVV.UtilsIL`) removed from LTCNodes — these are .NET Framework GAC/SDK auto-includes not applicable to .NET 10.

## Code Fixes

### ExampleDecode\Program.cs
- **CS0227**: Re-added `<AllowUnsafeBlocks>true</AllowUnsafeBlocks>` to the main PropertyGroup (had been in a conditional Debug-only group that was removed during project cleanup). `waveIn_DataAvailable` method uses `unsafe` pointer arithmetic.
- **CA1416** (x5): Added `[SupportedOSPlatform("windows")]` attribute to `FileLoadExample()` method — WinForms `OpenFileDialog` APIs are annotated as Windows-only. Project already targets `net10.0-windows` so this is correct and suppresses the analyzer warnings.
- Added `using System.Runtime.Versioning;` for the `SupportedOSPlatform` attribute.

### ExampleDecode\ExampleDecode.csproj
- Removed `<ImportWindowsDesktopTargets>true</ImportWindowsDesktopTargets>` — this was a legacy WPF/WinForms workaround for old SDK-style projects; the Windows Desktop SDK is automatically included when targeting `net10.0-windows` with `UseWindowsForms=true`.

## Build Results

| Project | Config | Result |
|---------|--------|--------|
| LTCNodes | Debug | ✅ Succeeded |
| ExampleEncode | Debug | ✅ Succeeded |
| ExampleDecode | Debug | ✅ Succeeded |

## Remaining Warnings (pre-existing)
- **MSB3270** (all three projects): x86 `LTCSharp.dll` vs AnyCPU managed projects, and AMD64 `SlimDX` — pre-existing architecture mismatch, no runtime impact in the expected VVVV x86 deployment scenario.
- **NU1701** (LTCNodes only): VVVV packages restored via .NET Framework compatibility fallback — expected and intentional per upgrade strategy.
- **LNK4075** (from LTCSharp build): Pre-existing from libltc.lib debug info format.

## Files Modified
- `LTCNodes/LTCNodes.csproj` — TFM, removed framework References, removed redundant packages
- `ExampleEncode/ExampleEncode.csproj` — TFM, removed legacy References, removed all VVVV + redundant packages
- `ExampleDecode/ExampleDecode.csproj` — TFM, AllowUnsafeBlocks, removed ImportWindowsDesktopTargets, removed legacy References, removed all VVVV + redundant packages
- `ExampleDecode/Program.cs` — Added SupportedOSPlatform attribute and using directive

## Done When Verification
- [x] LTCNodes builds targeting `net10.0-windows`
- [x] ExampleEncode builds targeting `net10.0-windows`
- [x] ExampleDecode builds targeting `net10.0-windows`
- [x] No VVVV APIs in ExampleDecode or ExampleEncode (verified by source scan — zero matches)
- [x] All framework-redundant packages removed from all three projects
- [x] No new warnings beyond pre-existing MSB3270/NU1701/LNK4075
