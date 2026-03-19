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

        // Candidate plugin folders: env var PLUGIN_FOLDER, cartella corrente/plugins, ../bin/debug/net8.0/plugins (se migrations in altro progetto)
        var candidates = new[]
        {
            Environment.GetEnvironmentVariable("PLUGIN_FOLDER"),
            Path.Combine(Directory.GetCurrentDirectory(), "plugins"),
            Path.Combine(Directory.GetCurrentDirectory(), "bin\\Debug\\net8.0\\plugins")
        }.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct();

        var assemblies = candidates.SelectMany(c =>
        {
            try
            {
                return PluginLoader.LoadPlugins(c);
            }
            catch
            {
                return Enumerable.Empty<System.Reflection.Assembly>();
            }
        }).ToArray();

        return new MyDbContext(builder.Options, assemblies);
    }
}