using App.Abstractions;
using McMaster.NETCore.Plugins;
using System.Reflection;

namespace App.Host.PluginManagement;

// Mantiene i loader in memoria per evitare che i plugin vengano collected
public class PluginManager : IDisposable
{
	private readonly List<PluginLoader> _loaders = [];
	private readonly List<Assembly> _assemblies = [];

	public IReadOnlyList<Assembly> Assemblies => _assemblies.AsReadOnly();

	public void LoadPluginsFromFolder(string pluginFolder, Func<string, bool>? filter = null)
	{
		if (string.IsNullOrWhiteSpace(pluginFolder) || !Directory.Exists(pluginFolder))
		{
			return;
		}

		var dlls = Directory.GetFiles(pluginFolder, "*.dll", SearchOption.TopDirectoryOnly).Where(p => filter == null || filter(p));

		foreach (var dll in dlls)
		{
			try
			{
				var loader = PluginLoader.CreateFromAssemblyFile(assemblyFile: Path.GetFullPath(dll), sharedTypes: [typeof(IEntity)]);

				_loaders.Add(loader);

				var asm = loader.LoadDefaultAssembly();

				//var loader = PluginLoader.CreateFromAssemblyFile(assemblyFile: Path.GetFullPath(dll), sharedTypes: [typeof(IEntity)]);
				//var asm = loader.LoadDefaultAssembly();

				if (asm != null)
				{
					_assemblies.Add(asm);
					Console.WriteLine($"Plugin loaded: {asm.FullName}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Plugin load failed for {dll}: {ex.Message}");
			}
		}
	}

	public void Dispose()
	{
		foreach (var loader in _loaders)
		{
			try { loader.Dispose(); }
			catch { }
		}

		_loaders.Clear();
		_assemblies.Clear();
	}
}