using App.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Host.Data;

public static class ModelBuilderExtensions
{
	public static void ApplyEntityTypeConfigurationsFromAssemblies(this ModelBuilder modelBuilder, IEnumerable<Assembly> assemblies)
	{
		foreach (var asm in assemblies)
		{
			try
			{
				modelBuilder.ApplyConfigurationsFromAssembly(asm);
			}
			catch
			{
				// Ignora assembly che non contengono configurazioni EF corrette
			}
		}
	}

	public static void RegisterEntitiesFromAssemblies(this ModelBuilder modelBuilder, IEnumerable<Assembly> assemblies)
	{
		var entityTypes = assemblies.SelectMany(a =>
		{
			try
			{
				return a.DefinedTypes;
			}
			catch
			{
				return [];
			}
		}).Where(ti => ti.IsClass && !ti.IsAbstract && ImplementsIEntity(ti)).Select(ti => ti.AsType()).Distinct();

		foreach (var type in entityTypes)
		{
			modelBuilder.Entity(type);
		}
	}

	private static bool ImplementsIEntity(TypeInfo ti)
	{
		return ti.ImplementedInterfaces.Any(i => i == typeof(IEntity));
	}
}