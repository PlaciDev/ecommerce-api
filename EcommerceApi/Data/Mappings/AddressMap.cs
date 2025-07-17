using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Data.Mappings
{
    public class AddressMap : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Street)
                .IsRequired()
                .HasColumnName("Street")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100);

            builder.Property(x => x.City)
                .IsRequired()
                .HasColumnName("City")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(50);

            builder.Property(x => x.State)
                .IsRequired()
                .HasColumnName("State")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(50);

             builder.Property(x => x.ZipCode)
                .IsRequired()
                .HasColumnName("ZipCode")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(20);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Addresses)
                .OnDelete(DeleteBehavior.Cascade);






        }
    }
}
