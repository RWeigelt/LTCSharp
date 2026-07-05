# 02-sdk-conversion: Convert ExampleDecode to SDK-style project format

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
