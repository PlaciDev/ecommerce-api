using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Data.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasColumnName("Description")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnName("Price")
                .HasColumnType("DECIMAL(18,2)");

            builder.Property(x => x.Stock)
                .IsRequired()
                .HasColumnName("Stock")
                .HasColumnType("INTEGER");

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.OrderItems)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
