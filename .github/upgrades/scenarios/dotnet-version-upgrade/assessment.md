# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
  - [Binding Redirect Configuration](#binding-redirect-configuration)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [ExampleDecode.csproj](#exampledecodecsproj)
  - [R:\LTCSHarp\libltc\libltc.vcxproj](#r:ltcsharplibltclibltcvcxproj)
  - [R:\LTCSHarp\LTCSharp\LTCSharp.vcxproj](#r:ltcsharpltcsharpltcsharpvcxproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 3 | 2 require upgrade |
| Total NuGet Packages | 0 | All compatible |
| Total Code Files | 2 |  |
| Total Code Files with Incidents | 3 |  |
| Total Lines of Code | 150 |  |
| Total Number of Issues | 22 |  |
| Estimated LOC to modify | 7+ | at least 4,7% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Binding Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: | :--- |
| [ExampleDecode.csproj](#exampledecodecsproj) | net48 | 🟢 Low | 12 | 7 | 0 | 7+ | ClassicWinForms, Sdk Style = False |
| [R:\LTCSHarp\libltc\libltc.vcxproj](#r:ltcsharplibltclibltcvcxproj) |  | ✅ None | 0 | 0 | 0 |  | ClassicDotNetApp, Sdk Style = False |
| [R:\LTCSHarp\LTCSharp\LTCSharp.vcxproj](#r:ltcsharpltcsharpltcsharpvcxproj) | net48 | 🟢 Low | 0 | 0 | 0 |  | ClassicClassLibrary, Sdk Style = False |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 0 | 0,0% |
| ⚠️ Incompatible | 0 | 0,0% |
| 🔄 Upgrade Recommended | 0 | 0,0% |
| ***Total NuGet Packages*** | ***0*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 7 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 80 |  |
| ***Total APIs Analyzed*** | ***87*** |  |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |
| Windows Forms | 7 | 100,0% | Windows Forms APIs for building Windows desktop applications with traditional Forms-based UI that are available in .NET on Windows. Enable Windows Desktop support: Option 1 (Recommended): Target net9.0-windows; Option 2: Add <UseWindowsDesktop>true</UseWindowsDesktop>; Option 3 (Legacy): Use Microsoft.NET.Sdk.WindowsDesktop SDK. |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |
| P:System.Windows.Forms.FileDialog.FileName | 1 | 14,3% | Binary Incompatible |
| T:System.Windows.Forms.DialogResult | 1 | 14,3% | Binary Incompatible |
| M:System.Windows.Forms.CommonDialog.ShowDialog | 1 | 14,3% | Binary Incompatible |
| P:System.Windows.Forms.FileDialog.RestoreDirectory | 1 | 14,3% | Binary Incompatible |
| P:System.Windows.Forms.FileDialog.Filter | 1 | 14,3% | Binary Incompatible |
| T:System.Windows.Forms.OpenFileDialog | 1 | 14,3% | Binary Incompatible |
| M:System.Windows.Forms.OpenFileDialog.#ctor | 1 | 14,3% | Binary Incompatible |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>⚙️&nbsp;ExampleDecode.csproj</b><br/><small>net48</small>"]
    P2["<b>⚙️&nbsp;LTCSharp.vcxproj</b><br/><small>net48</small>"]
    P3["<b>⚙️&nbsp;libltc.vcxproj</b><br/><small></small>"]
    P1 --> P2
    P2 --> P3
    click P1 "#exampledecodecsproj"
    click P2 "#r:ltcsharpltcsharpltcsharpvcxproj"
    click P3 "#r:ltcsharplibltclibltcvcxproj"

```

## Project Details

<a id="exampledecodecsproj"></a>
### ExampleDecode.csproj

#### Project Info

- **Current Target Framework:** net48
- **Proposed Target Framework:** net10.0-windows
- **SDK-style**: False
- **Project Kind:** ClassicWinForms
- **Dependencies**: 1
- **Dependants**: 0
- **Number of Files**: 2
- **Number of Files with Incidents**: 2
- **Lines of Code**: 150
- **Estimated LOC to modify**: 7+ (at least 4,7% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["ExampleDecode.csproj"]
        MAIN["<b>⚙️&nbsp;ExampleDecode.csproj</b><br/><small>net48</small>"]
        click MAIN "#exampledecodecsproj"
    end
    subgraph downstream["Dependencies (1"]
        P2["<b>⚙️&nbsp;LTCSharp.vcxproj</b><br/><small>net48</small>"]
        click P2 "#r:ltcsharpltcsharpltcsharpvcxproj"
    end
    MAIN --> P2

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 7 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 80 |  |
| ***Total APIs Analyzed*** | ***87*** |  |

#### Project Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |
| Windows Forms | 7 | 100,0% | Windows Forms APIs for building Windows desktop applications with traditional Forms-based UI that are available in .NET on Windows. Enable Windows Desktop support: Option 1 (Recommended): Target net9.0-windows; Option 2: Add <UseWindowsDesktop>true</UseWindowsDesktop>; Option 3 (Legacy): Use Microsoft.NET.Sdk.WindowsDesktop SDK. |

<a id="r:ltcsharplibltclibltcvcxproj"></a>
### R:\LTCSHarp\libltc\libltc.vcxproj

#### Project Info

- **Current Target Framework:** ✅
- **SDK-style**: False
- **Project Kind:** ClassicDotNetApp
- **Dependencies**: 0
- **Dependants**: 1
- **Number of Files**: 0
- **Lines of Code**: 0
- **Estimated LOC to modify**: 0+ (at least 0,0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P2["<b>⚙️&nbsp;LTCSharp.vcxproj</b><br/><small>net48</small>"]
        click P2 "#r:ltcsharpltcsharpltcsharpvcxproj"
    end
    subgraph current["libltc.vcxproj"]
        MAIN["<b>⚙️&nbsp;libltc.vcxproj</b><br/><small></small>"]
        click MAIN "#r:ltcsharplibltclibltcvcxproj"
    end
    P2 --> MAIN

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

#### Project Package References

| Package | Type | Current Version | Suggested Version | Description |
| :--- | :---: | :---: | :---: | :--- |

<a id="r:ltcsharpltcsharpltcsharpvcxproj"></a>
### R:\LTCSHarp\LTCSharp\LTCSharp.vcxproj

#### Project Info

- **Current Target Framework:** net48
- **Proposed Target Framework:** net10.0
- **SDK-style**: False
- **Project Kind:** ClassicClassLibrary
- **Dependencies**: 1
- **Dependants**: 1
- **Number of Files**: 0
- **Number of Files with Incidents**: 1
- **Lines of Code**: 0
- **Estimated LOC to modify**: 0+ (at least 0,0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P1["<b>⚙️&nbsp;ExampleDecode.csproj</b><br/><small>net48</small>"]
        click P1 "#exampledecodecsproj"
    end
    subgraph current["LTCSharp.vcxproj"]
        MAIN["<b>⚙️&nbsp;LTCSharp.vcxproj</b><br/><small>net48</small>"]
        click MAIN "#r:ltcsharpltcsharpltcsharpvcxproj"
    end
    subgraph downstream["Dependencies (1"]
        P3["<b>⚙️&nbsp;libltc.vcxproj</b><br/><small></small>"]
        click P3 "#r:ltcsharplibltclibltcvcxproj"
    end
    P1 --> MAIN
    MAIN --> P3

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

#### Project Package References

| Package | Type | Current Version | Suggested Version | Description |
| :--- | :---: | :---: | :---: | :--- |

