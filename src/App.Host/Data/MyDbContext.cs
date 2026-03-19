using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace App.Host.Data;

public class MyDbContext(DbContextOptions<MyDbContext> options, IEnumerable<Assembly> pluginAssemblies) : DbContext(options)
{
    private readonly IEnumerable<Assembly> _pluginAssemblies = pluginAssemblies ?? [];

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyEntityTypeConfigurationsFromAssemblies(_pluginAssemblies);
        modelBuilder.RegisterEntitiesFromAssemblies(_pluginAssemblies);

        base.OnModelCreating(modelBuilder);
    }
}