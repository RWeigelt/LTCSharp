# 05-final-validation: Full solution validation and post-upgrade cleanup

Build the complete solution in both Debug and Release configurations and confirm there are no remaining errors or warnings introduced by the upgrade. Run any test projects if present. Verify that the `ExampleDecode` and `ExampleEncode` applications launch correctly against the new .NET 10 runtime.

Document deferred recommendations for the user:
- **VVVV stub resolution** (if stubs were created in task 04): Each `// TODO: resolve VVVV stub` comment identifies code that needs a real replacement; address after confirming the app runs correctly.
- **Central Package Management (CPM)**: All projects are now SDK-style and on a single TFM family — CPM can be added cleanly with `Directory.Packages.props` without VersionOverride friction. Recommended as a separate follow-up.
- **Nullable Reference Types**: Enable `<Nullable>enable</Nullable>` per project as a separate effort once the migration is stable.

Commit all changes on the `net10` working branch.

**Done when**: Full solution builds in Debug and Release with no errors; any example apps launch successfully; deferred items documented; all changes committed to the `net10` branch.
