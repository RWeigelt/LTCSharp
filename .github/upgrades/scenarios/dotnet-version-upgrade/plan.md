# .NET Version Upgrade Plan

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously in a single operation.
**Rationale**: Single C# project (ExampleDecode, net48), no dependency phasing needed, small codebase (150 LOC), low complexity assessment.

## Overview

**Target**: ExampleDecode — Windows Forms application (net48 → net10.0-windows)
**Scope**: 1 C# project; C++ projects (LTCSharp, libltc) are excluded

## Tasks

### 01-prerequisites: Verify upgrade prerequisites

Confirm the .NET 10 SDK is installed and compatible before any project changes are made.
Check whether a `global.json` file exists in the repository root or any parent directory
that could pin the SDK version to an incompatible range and prevent the upgraded project
from building.

**Done when**: .NET 10 SDK confirmed installed and any `global.json` constraints are verified
to be compatible with net10.0 (or updated if needed).

---

### 02-sdk-conversion: Convert ExampleDecode to SDK-style project format

ExampleDecode.csproj currently uses the old-style (non-SDK) project format (`ToolsVersion` attribute,
explicit `<Import>` targets). It must be converted to SDK-style format before the TFM can be changed,
as the conversion tooling operates on the current framework and has a distinct failure surface from the
TFM upgrade. The project likely uses a `packages.config` file; this must be migrated to
`<PackageReference>` format as part of this conversion.

The conversion should stay on net48 — this task is purely a structural change to the project file
format. No TFM changes, no package version updates.

Assessment context: ClassicWinForms project, SDK-style = false, packages.config likely present.

**Done when**: ExampleDecode.csproj is in SDK-style format, still targeting net48, solution builds
without errors, `packages.config` (if present) has been replaced with `<PackageReference>` items.

---

### 03-upgrade-example-decode: Upgrade ExampleDecode to net10.0-windows

Change the target framework from `net48` to `net10.0-windows`. The `-windows` suffix is
required — the project uses Windows Forms APIs that are only available in the Windows-specific
TFM. Without it, the 7 Windows Forms APIs flagged in the assessment (OpenFileDialog,
FileDialog properties, ShowDialog, DialogResult) will not resolve.

Assessment flagged 12 NuGet package issues (incompatible, upgrade recommended, or now
framework-provided). Many packages from the old-style format may become redundant once
targeting net10.0-windows (Windows Forms and Windows Desktop APIs ship with the framework).
Review each package reference after the TFM change: remove those now provided by the framework,
update those with newer compatible versions, and investigate any that remain incompatible.

After package cleanup, build the solution and fix all compilation errors arising from
the net48 → net10.0 breaking changes. The assessment found 7 binary-incompatible Windows Forms
APIs; these should resolve automatically with the correct `-windows` TFM. Address any
remaining build errors.

**Done when**: ExampleDecode targets `net10.0-windows`, solution builds with zero errors
and zero warnings, all packages are either removed (framework-provided), updated to a
compatible version, or documented as intentionally retained.

---

### 04-validation: Final solution build and validation

Perform a clean solution build to confirm all projects build together correctly — both the
upgraded ExampleDecode and the unchanged C++ projects (LTCSharp, libltc). Verify no
regressions were introduced in the C++ side as a side effect of the project structure
changes.

Run any existing automated tests. If no tests exist, document that fact. Record build
warnings, if any, and confirm they are expected or file them as follow-up items.

**Done when**: Full solution builds cleanly with zero errors. C++ projects are unaffected.
Any test suite passes (or absence of tests is confirmed and noted).
