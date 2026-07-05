# Upgrade Plan — LTCSharp → .NET 10

## Solution Overview

| Project | Kind | Current TFM | Target TFM | Tier |
|---------|------|-------------|------------|------|
| libltc\libltc.vcxproj | Native C++ | — | — | 0 (no change) |
| LTCSharp\LTCSharp.vcxproj | C++/CLI | net48 | net10.0 | 1 |
| LTCNodes\LTCNodes.csproj | Class Library | net48 | net10.0 | 2 |
| ExampleEncode\ExampleEncode.csproj | Console App | net48 | net10.0 | 2 |
| ExampleDecode\ExampleDecode.csproj | WinForms App | net48 | net10.0-windows | 2 |

## Selected Strategy

**Bottom-Up (Dependency-First)** — Upgrade from leaf nodes to root applications, tier by tier.  
**Rationale**: 4 .NET projects spanning a 2-tier dependency graph crossing the .NET Framework → modern .NET boundary. Fixed strategy for Framework migrations with multiple projects.

```
Tier 2: [LTCNodes]   [ExampleEncode]   [ExampleDecode]
			  ↓              ↓                ↓
Tier 1:           [LTCSharp (C++/CLI)]
						 ↓
Tier 0:          [libltc (native C++)] ← no changes
```

**Execution Constraints:**
- Strict tier ordering: Tier N must complete and validate before Tier N+1 begins
- SDK-style conversion is always a separate task from TFM upgrade — never merged
- LTCSharp (C++/CLI) upgrades before any Tier 2 project touches TFM; Tier 2 projects cannot build against net48 once Tier 1 moves to net10.0
- Package Management: Per-project (no CPM during migration); CPM deferred to post-migration
- Unsupported Packages: Defer Resolution — incompatible VVVV packages in ExampleDecode get minimal stubs; dedicated subtask resolves them
- Unsupported API Handling: Fix Inline — 7 WinForms API breaking changes in ExampleDecode resolved in the Tier 2 task
- Nullable Reference Types: Leave disabled throughout upgrade; enable separately post-migration

---

## Tasks

### 01-prerequisites: Verify upgrade prerequisites

Confirm the environment is ready for the upgrade before any project changes are made. The .NET 10 SDK has already been validated as present. This task verifies the remaining prerequisites: `global.json` compatibility, absence of blocking SDK pins, and confirmation that the `libltc` native C++ project (Tier 0) requires no .NET framework changes whatsoever.

Review the solution's `.github/upgrades/scenarios/dotnet-version-upgrade/assessment.md` for any issues not captured in the plan. Check whether any CI/build scripts reference `net48` explicitly and would need updating.

**Done when**: .NET 10 SDK confirmed compatible with any `global.json` present; `libltc` confirmed as a native C++ project with no TFM change needed; no blocking environment issues identified.

---

### 02-sdk-conversion: Convert C# projects to SDK-style format

Convert the three non-SDK-style C# project files — `LTCNodes\LTCNodes.csproj`, `ExampleEncode\ExampleEncode.csproj`, and `ExampleDecode\ExampleDecode.csproj` — to SDK-style format. This is a structural change only: **target framework stays at net48**. The TFM upgrade happens in a later task.

All three projects currently use the old `<Project ToolsVersion="...">` format with `packages.config` for NuGet references. The SDK-style conversion tool handles the format change and migrates `packages.config` to `<PackageReference>` entries inside the project file. After conversion, verify each project builds and restores packages successfully on net48 before proceeding.

`LTCSharp\LTCSharp.vcxproj` (C++/CLI) and `libltc\libltc.vcxproj` (native C++) are excluded — they use the vcxproj format which is outside the scope of .NET SDK-style conversion.

**Done when**: All three C# projects (`LTCNodes`, `ExampleEncode`, `ExampleDecode`) use SDK-style csproj format; `packages.config` files removed; all three build successfully on `net48`; solution restores without errors.

---

### 03-ltcsharp-retarget: Retarget LTCSharp C++/CLI to .NET 10 (Tier 1)

Upgrade `LTCSharp\LTCSharp.vcxproj` from `net48` to `net10.0`. This is a C++/CLI project — a managed C++ wrapper around the native `libltc` library. C++/CLI on modern .NET requires switching CLR support from `<CLRSupport>true</CLRSupport>` (which implies .NET Framework) to `<CLRSupport>NetCore</CLRSupport>`, and adding the `<TargetFramework>net10.0-windows</TargetFramework>` property. The MSVC toolset version must support .NET Core CLR (VS 2019 16.4+ / MSVC 14.24+, already satisfied by VS 2026).

Key risks: C++/CLI on .NET Core is Windows-only (net10.0-windows effective target), any `#using` of .NET Framework assemblies must be replaced with .NET 10 equivalents, and the project may need explicit `/MT` or `/MD` runtime library settings reviewed. The assessment flagged only one issue (Project.0002 — TFM change), suggesting the C++ code itself has no managed API incompatibilities.

After this change, Tier 2 C# projects referencing LTCSharp will temporarily fail to build (they still reference net48 LTCSharp). This is expected and resolved in the next task.

**Done when**: `LTCSharp.vcxproj` builds successfully targeting `net10.0-windows`; the managed C++/CLI assembly is loadable by a .NET 10 consumer; `libltc` native dependency links correctly.

---

### 04-tier2-upgrade: Upgrade Tier 2 projects to .NET 10

Upgrade all three Tier 2 projects to .NET 10. This task covers TFM changes, package cleanup, incompatible package stubs, and WinForms API fixes. Because all three projects depend on LTCSharp (now net10.0), they cannot build on net48 — proceed with all three immediately after Tier 1 validates.

**LTCNodes\LTCNodes.csproj** → `net10.0`. Remove the five framework-redundant packages now included in the .NET 10 runtime: `Microsoft.Win32.Registry`, `System.Buffers`, `System.Memory`, `System.Numerics.Vectors`, `System.Security.Principal.Windows`. VVVV packages (`VVVV.Core`, `VVVV.PluginInterfaces`, `VVVV.SlimDX`, `VVVV.System.ComponentModel.Composition.Codeplex`, `VVVV.Utils`) are compatible with net10.0 — retain at current versions.

**ExampleEncode\ExampleEncode.csproj** → `net10.0`. Same five framework-redundant packages removed. VVVV packages compatible — retain. No API breaking changes flagged.

**ExampleDecode\ExampleDecode.csproj** → `net10.0-windows` (WinForms requires Windows TFM). This is the most complex project: (1) the same five framework-redundant packages must be removed; (2) four incompatible VVVV packages (`VVVV.Core`, `VVVV.PluginInterfaces`, `VVVV.System.ComponentModel.Composition.Codeplex`, `VVVV.Utils`) must be removed — first check whether ExampleDecode's code actually calls any VVVV APIs; if not, remove directly; if yes, generate minimal type stubs and create a stub-resolution subtask; (3) fix the 7 binary-incompatible WinForms API calls inline — these are typically mechanical changes (event handler signature adjustments, property renames) documented in the .NET breaking changes reference.

Assessment reference for ExampleDecode API issues: `assessment.md`, project scope `ExampleDecode\ExampleDecode.csproj`, category `Api.0001`.

**Done when**: All three projects build targeting .NET 10 with no errors; framework-redundant package references removed; ExampleDecode either has no VVVV API calls (packages removed cleanly) or has compilable stubs with `// TODO: resolve VVVV stub` markers; all 7 WinForms API breaking changes resolved inline; solution builds cleanly.

---

### 05-final-validation: Full solution validation and post-upgrade cleanup

Build the complete solution in both Debug and Release configurations and confirm there are no remaining errors or warnings introduced by the upgrade. Run any test projects if present. Verify that the `ExampleDecode` and `ExampleEncode` applications launch correctly against the new .NET 10 runtime.

Document deferred recommendations for the user:
- **VVVV stub resolution** (if stubs were created in task 04): Each `// TODO: resolve VVVV stub` comment identifies code that needs a real replacement; address after confirming the app runs correctly.
- **Central Package Management (CPM)**: All projects are now SDK-style and on a single TFM family — CPM can be added cleanly with `Directory.Packages.props` without VersionOverride friction. Recommended as a separate follow-up.
- **Nullable Reference Types**: Enable `<Nullable>enable</Nullable>` per project as a separate effort once the migration is stable.

Commit all changes on the `net10` working branch.

**Done when**: Full solution builds in Debug and Release with no errors; any example apps launch successfully; deferred items documented; all changes committed to the `net10` branch.
