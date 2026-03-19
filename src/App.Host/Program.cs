//Console.WriteLine("Hello, World!");
using System.Reflection;
using App.Host.Data;
using App.Host.PluginManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var pluginFolder = Environment.GetEnvironmentVariable("PLUGIN_FOLDER") ?? Path.Combine(AppContext.BaseDirectory, "plugins");
        var pluginAssemblies = PluginLoader.LoadPlugins(pluginFolder).ToArray();

        services.AddSingleton<IEnumerable<Assembly>>(pluginAssemblies);

        var conn = context.Configuration.GetConnectionString("DefaultConnection") ?? "Server=(localdb)\\mssqllocaldb;Database=MyApp;Trusted_Connection=True;";

        services.AddDbContext<MyDbContext>(options =>
        {
            options.UseSqlServer(conn);
        });
    }).Build();

Console.WriteLine("Host built. Copia i plugin nella cartella plugins e usa dotnet ef con --startup-project se necessario.");