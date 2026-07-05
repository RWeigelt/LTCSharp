# 03-ltcsharp-retarget: Retarget LTCSharp C++/CLI to .NET 10 (Tier 1)

Upgrade `LTCSharp\LTCSharp.vcxproj` from `net48` to `net10.0`. This is a C++/CLI project — a managed C++ wrapper around the native `libltc` library. C++/CLI on modern .NET requires switching CLR support from `<CLRSupport>true</CLRSupport>` (which implies .NET Framework) to `<CLRSupport>NetCore</CLRSupport>`, and adding the `<TargetFramework>net10.0-windows</TargetFramework>` property. The MSVC toolset version must support .NET Core CLR (VS 2019 16.4+ / MSVC 14.24+, already satisfied by VS 2026).

Key risks: C++/CLI on .NET Core is Windows-only (net10.0-windows effective target), any `#using` of .NET Framework assemblies must be replaced with .NET 10 equivalents, and the project may need explicit `/MT` or `/MD` runtime library settings reviewed. The assessment flagged only one issue (Project.0002 — TFM change), suggesting the C++ code itself has no managed API incompatibilities.

After this change, Tier 2 C# projects referencing LTCSharp will temporarily fail to build (they still reference net48 LTCSharp). This is expected and resolved in the next task.

**Done when**: `LTCSharp.vcxproj` builds successfully targeting `net10.0-windows`; the managed C++/CLI assembly is loadable by a .NET 10 consumer; `libltc` native dependency links correctly.
