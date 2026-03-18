using App.Host.PluginManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace App.Host.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
{
	public MyDbContext CreateDbContext(string[] args)
	{
		var builder = new DbContextOptionsBuilder<MyDbContext>();
		var conn = Environment.GetEnvironmentVariable("DESIGNTIME_CONNECTION") ?? "Server=(localdb)\\mssqllocaldb;Database=MyApp.DesignTime;Trusted_Connection=True;";

		builder.UseSqlServer(conn);

		// Candidate plugin folders (puoi aggiungere altri)
		var candidates = new[]
		{
				Environment.GetEnvironmentVariable("PLUGIN_FOLDER"),
				Path.Combine(Directory.GetCurrentDirectory(), "plugins"),
				Path.Combine(Directory.GetCurrentDirectory(), "bin\\Debug\\net8.0\\plugins")
			}.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct();

		var pluginManager = new PluginManager();

		foreach (var c in candidates)
		{
			try
			{
				pluginManager.LoadPluginsFromFolder(pluginFolder: c);
			}
			catch
			{ }
		}

		return new MyDbContext(builder.Options, pluginManager.Assemblies);
	}
}