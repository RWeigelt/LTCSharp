# 02-sdk-conversion: Convert C# projects to SDK-style format

Convert the three non-SDK-style C# project files — `LTCNodes\LTCNodes.csproj`, `ExampleEncode\ExampleEncode.csproj`, and `ExampleDecode\ExampleDecode.csproj` — to SDK-style format. This is a structural change only: **target framework stays at net48**. The TFM upgrade happens in a later task.

All three projects currently use the old `<Project ToolsVersion="...">` format with `packages.config` for NuGet references. The SDK-style conversion tool handles the format change and migrates `packages.config` to `<PackageReference>` entries inside the project file. After conversion, verify each project builds and restores packages successfully on net48 before proceeding.

`LTCSharp\LTCSharp.vcxproj` (C++/CLI) and `libltc\libltc.vcxproj` (native C++) are excluded — they use the vcxproj format which is outside the scope of .NET SDK-style conversion.

**Done when**: All three C# projects (`LTCNodes`, `ExampleEncode`, `ExampleDecode`) use SDK-style csproj format; `packages.config` files removed; all three build successfully on `net48`; solution restores without errors.
