using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Data.Mappings
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Date)
                .IsRequired()
                .HasColumnName("Date")
                .HasColumnType("DATE");

            builder.Property(x => x.TotalAmount)
                .IsRequired()
                .HasColumnName("TotalAmount")
                .HasColumnType("DECIMAL(18,2)");

            builder.Property(x => x.Status)
                .IsRequired()
                .HasColumnName("Status")
                .HasColumnType("VARCHAR")
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Payment)
                .WithOne(x => x.Order)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
