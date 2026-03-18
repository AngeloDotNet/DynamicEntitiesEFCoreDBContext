using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PluginProduct.Entities;

namespace PluginProduct.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
	public void Configure(EntityTypeBuilder<Product> builder)
	{
		builder.ToTable("Products");
		builder.HasKey(p => p.Id);
		builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
		builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
	}
}