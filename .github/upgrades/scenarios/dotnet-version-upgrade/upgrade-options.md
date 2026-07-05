# Upgrade Options — LTCSharp

Assessment: 5 projects (3 C# net48, 1 C++/CLI net48, 1 native C++); target net10.0 / net10.0-windows; 4 incompatible VVVV packages; 7 WinForms binary-incompatible APIs

## Strategy

### Upgrade Strategy
Solution has multiple .NET Framework projects crossing the Framework → modern .NET boundary, requiring tier-by-tier validation. Bottom-Up is fixed for this configuration.

| Value | Description |
|-------|-------------|
| **Bottom-Up** (selected) | Upgrade leaf-node libraries first (LTCSharp C++/CLI), then Tier 2 projects (LTCNodes, ExampleEncode, ExampleDecode). Each tier validated before advancing. Fixed for .NET Framework → modern .NET with multiple projects. |

## Project Structure

### Project Approach
LTCNodes is the only class library; all its consumers (ExampleDecode, ExampleEncode) are migrating simultaneously — no Framework dependents will remain.

| Value | Description |
|-------|-------------|
| **In-place** (selected) | Replace each project's TFM directly. No multi-targeting needed because all consumers migrate together. |
| Multi-targeting | Adds new TFM alongside existing (net48;net10.0). Only needed when some consumers remain on .NET Framework after this upgrade. |

### Package Management
All three C# projects use legacy packages.config and non-SDK-style project files; upgrade crosses the .NET Framework → modern .NET boundary.

| Value | Description |
|-------|-------------|
| **Per-Project (defer CPM to post-migration)** (selected) | Each project retains its own package versions during the active migration. CPM is registered as a post-migration recommendation once all projects are SDK-style and on a single TFM. |
| Central Package Management (CPM) | Creates Directory.Packages.props and centralises versions. Not recommended here — old-style project files and the Framework→Core boundary create VersionOverride friction. |

## Compatibility

### Unsupported Packages
ExampleDecode references 4 VVVV packages (VVVV.Core, VVVV.PluginInterfaces, VVVV.System.ComponentModel.Composition.Codeplex, VVVV.Utils) that are incompatible with net10.0-windows and have no known direct replacement.

| Value | Description |
|-------|-------------|
| **Defer Resolution** (selected) | Remove incompatible package references; generate minimal type stubs to keep the project compiling; follow-up subtasks handle real resolution (which may be simply removing unused references if the code doesn't call VVVV APIs). |
| Resolve Inline | Research and resolve all incompatible packages within the same task. Recommended for 1–3 packages; 4 packages with Bottom-Up strategy favours deferral. |

### Unsupported API Handling
ExampleDecode has 7 binary-incompatible WinForms API calls flagged by the assessment. All 7 are confined to a single project; WinForms changes between .NET Framework 4.8 and .NET 10 are typically mechanical (signature or event-handler adjustments).

| Value | Description |
|-------|-------------|
| **Fix Inline** (selected) | Resolve every WinForms API change within the ExampleDecode task. No deferred stubs. |
| Defer Complex Changes | Apply simple replacements inline; stub complex ones and create follow-up subtasks. Appropriate when >5 complex changes span multiple projects — not the case here. |

## Modernization

### Nullable Reference Types
Migration is already demanding (WinForms API breaking changes, C++/CLI project retargeting, incompatible packages); enabling NRTs simultaneously would add noise without benefit.

| Value | Description |
|-------|-------------|
| **Leave Disabled** (selected) | Does not enable nullable reference types. Enable separately after the migration stabilises as a distinct effort. |
| Enable Nullable Reference Types | Adds `<Nullable>enable</Nullable>` to project files. Recommended for low-complexity upgrades on small codebases. |
