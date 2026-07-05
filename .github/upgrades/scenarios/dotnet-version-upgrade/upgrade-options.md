# Upgrade Options — LTCSharp

Assessment: 1 C# project (ExampleDecode, net48, Windows Forms, not SDK-style), 2 C++ projects excluded from upgrade, 7 Windows Forms API issues (resolved by net10.0-windows TFM), 150 LOC

## Strategy

### Upgrade Strategy
Single .NET Framework project — no dependency graph to manage.

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Upgrade the project in a single atomic pass. No multi-targeting overhead. |

---

## Project Structure

### Project Approach
ExampleDecode targets net48 (ClassicWinForms). It has no downstream consumers — it is the application itself. In-place migration is the correct approach.

| Value | Description |
|-------|-------------|
| **In-place** (selected) | Replace the TFM directly. No multi-targeting needed since this project has no dependants. |
| Multi-targeting | Adds new TFM alongside net48. Not needed — no other projects consume ExampleDecode. |

---

## Modernization

### Nullable Reference Types
Target is net10.0-windows; ExampleDecode is a small C# codebase (150 LOC, 1 project, Low complexity). Enabling nullable reference types from the start is feasible.

| Value | Description |
|-------|-------------|
| Enable Nullable Reference Types | Adds `<Nullable>enable</Nullable>` to the project file. Small codebase means manageable warning volume. |
| **Leave Disabled** (selected) | Does not enable nullable. Enable separately after migration as a distinct effort. |
