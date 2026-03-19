## Dynamically reading DbContext entities with EFCore

> [!IMPORTANT]
> The McMaster.Plugins Nuget package is used in the version present in the CodeWithNugetPlugin branch, while the Main branch contains the version that uses reflection and not Nuget packages.

### Structure:

- App.Abstractions: Contains IEntity (marker interface).
- App.Host: Host application that loads plugins, registers assemblies for EF, and provides MyDbContext.
- PluginProduct (ExamplePlugin): Example plugin with Product and ProductConfiguration entities.

### Requirements:

- .NET SDK 8 (target projects can be updated)
- dotnet CLI

### NuGet packages:

- McMaster.NETCore.Plugins (used in the host to load isolated plugins)
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer (or other provider)

### Notes:

- Plugins must reference the same shared App.Abstractions assembly for the marker interface to work.
- The template provides both scripts and an optional MSBuild target example to automatically copy plugins.
- Customize the paths, framework targets, and DB providers to suit your needs.
