using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Data.Mappings
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasColumnName("PhoneNumber")
                .HasColumnType("INTEGER");

            builder.HasOne(x => x.User)
                .WithOne(x => x.CustomerProfile);

            builder.HasMany(x => x.Addresses)
                .WithOne(x => x.Customer)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Orders)
                .WithOne(x => x.Customer)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
