using System.Reflection;
using System.Runtime.Loader;

namespace App.Host.PluginManagement;

// Caricatore semplice che usa AssemblyLoadContext.Default.LoadFromAssemblyPath
public static class PluginLoader
{
    // Carica tutti i .dll da pluginFolder e restituisce gli Assembly caricati
    public static IEnumerable<Assembly> LoadPlugins(string pluginFolder, Func<string, bool>? filter = null)
    {
        if (string.IsNullOrWhiteSpace(pluginFolder) || !Directory.Exists(pluginFolder))
        {
            return Array.Empty<Assembly>();
        }

        var dlls = Directory
            .GetFiles(pluginFolder, "*.dll", SearchOption.TopDirectoryOnly)
            .Where(p => filter == null || filter(p));

        var loaded = new List<Assembly>();

        foreach (var path in dlls)
        {
            try
            {
                var fullPath = Path.GetFullPath(path);

                // Evita di ricaricare un assembly già presente nell'appdomain (by identity)
                var name = AssemblyName.GetAssemblyName(fullPath);
                var already = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .FirstOrDefault(a =>
                    {
                        try
                        {
                            return AssemblyName.ReferenceMatchesDefinition(a.GetName(), name);
                        }
                        catch
                        {
                            return false;
                        }
                    });

                if (already != null)
                {
                    loaded.Add(already);
                    continue;
                }

                // Carica l'assembly nel contesto di default (compatibile con EF Core)
                var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullPath);

                if (asm != null)
                {
                    loaded.Add(asm);
                }
            }
            catch (Exception ex)
            {
                // Loggare come preferisci; qui stampa su console
                Console.WriteLine($"Failed to load plugin '{path}': {ex.Message}");
            }
        }

        return loaded;
    }
}