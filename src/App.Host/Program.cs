//Console.WriteLine("Hello, World!");
using App.Host.Data;
using App.Host.PluginManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((context, services) =>
	{
		// CARTELLA PLUGIN: può essere sovrascritta con env var PLUGIN_FOLDER
		var pluginFolder = Environment.GetEnvironmentVariable("PLUGIN_FOLDER") ?? Path.Combine(AppContext.BaseDirectory, "plugins");

		var pluginManager = new PluginManager();
		pluginManager.LoadPluginsFromFolder(pluginFolder);

		services.AddSingleton(pluginManager);
		services.AddSingleton<IEnumerable<Assembly>>(sp => sp.GetRequiredService<PluginManager>().Assemblies);

		var conn = context.Configuration.GetConnectionString("DefaultConnection") ?? "Server=(localdb)\\mssqllocaldb;Database=MyApp;Trusted_Connection=True;";

		services.AddDbContext<MyDbContext>(options =>
		{
			options.UseSqlServer(conn);
		});
	})
	.Build();

Console.WriteLine("Host built. Run your application or use dotnet ef with --startup-project MyApp.Host");
