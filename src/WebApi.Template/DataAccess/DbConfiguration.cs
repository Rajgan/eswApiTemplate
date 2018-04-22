using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Template.Model.Domain;

namespace WebApi.Template.DataAccess
{
    public static class DbConfiguration
    {
        internal static void Configure_NameAddresses(EntityTypeBuilder<NameAddresses> entity)
        {
            entity.HasKey(e => e.NameAddressId);

            entity.ToTable("NameAddresses", "Parties");

            entity.HasIndex(e => e.Email)
                  .HasName("IX_NameAddresses_Email_BF_20160908");

            entity.HasIndex(e => new { e.NameAddressId, e.CountryIso })
                  .HasName("IX_NameAddresses_CountryIso_BF");

            entity.HasIndex(e => new { e.FirstName, e.LastName, e.Address1, e.Address2 })
                  .HasName("IX_NameAddresses");

            entity.Property(e => e.Address1)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(e => e.Address2).HasMaxLength(150);

            entity.Property(e => e.Address3).HasMaxLength(150);

            entity.Property(e => e.City).HasMaxLength(150);

            entity.Property(e => e.CountryIso)
                  .IsRequired()
                  .HasColumnType("char(2)");

            entity.Property(e => e.Email).HasMaxLength(100);

            entity.Property(e => e.FirstName).HasMaxLength(70);

            entity.Property(e => e.LastName).HasMaxLength(70);

            entity.Property(e => e.LastUpdateTime).HasColumnType("datetime");

            entity.Property(e => e.LastUpdateUser).HasMaxLength(256);

            entity.Property(e => e.Pobox)
                  .HasColumnName("POBox")
                  .HasMaxLength(50);

            entity.Property(e => e.PostalCode).HasMaxLength(50);

            entity.Property(e => e.Region).HasMaxLength(150);

            entity.Property(e => e.Telephone).HasMaxLength(150);

            entity.Property(e => e.Unit).HasMaxLength(100);
        }

    }
}
