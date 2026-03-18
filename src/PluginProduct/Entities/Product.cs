using App.Abstractions;

namespace PluginProduct.Entities;

public class Product : IEntity
{
	public int Id { get; set; }
	public string Name { get; set; } = default!;
	public decimal Price { get; set; }
}