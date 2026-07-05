# 03-upgrade-example-decode: Upgrade ExampleDecode to net10.0-windows

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
