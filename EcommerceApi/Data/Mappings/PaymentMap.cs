using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Data.Mappings
{
    public class PaymentMap : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payment");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Method)
                .IsRequired()
                .HasColumnName("Method")
                .HasColumnType("VARCHAR")
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.Date)
                .IsRequired()
                .HasColumnName("Date")
                .HasColumnType("DATE");

            builder.Property(x => x.Amount)
                .IsRequired()
                .HasColumnName("Amount")
                .HasColumnType("DECIMAL(18,2)");

            builder.Property(x => x.Status)
                .IsRequired()
                .HasColumnName("Status")
                .HasColumnType("VARCHAR")
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(x => x.Order)
                .WithOne(x => x.Payment)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
