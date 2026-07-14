using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Domain.Identity;

namespace Platform.Infrastructure.Persistence;

public sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.ToTable("user_accounts");
        builder.HasKey(account => account.Id);

        builder.Property(account => account.Id).HasColumnName("id");
        builder.Property(account => account.FirebaseUid)
            .HasColumnName("firebase_uid")
            .HasMaxLength(128)
            .IsRequired();
        builder.HasIndex(account => account.FirebaseUid)
            .IsUnique()
            .HasDatabaseName("ux_user_accounts_firebase_uid");

        builder.Property(account => account.Email)
            .HasColumnName("email")
            .HasMaxLength(320);
        builder.Property(account => account.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(80);
        builder.Property(account => account.Locale)
            .HasColumnName("locale")
            .HasMaxLength(16)
            .IsRequired();
        builder.Property(account => account.TimeZoneId)
            .HasColumnName("time_zone_id")
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(account => account.CityId).HasColumnName("city_id");
        builder.Property(account => account.Status)
            .HasColumnName("status")
            .HasConversion<short>();
        builder.Property(account => account.CreatedAt).HasColumnName("created_at");
        builder.Property(account => account.UpdatedAt).HasColumnName("updated_at");
        builder.Property(account => account.Version)
            .HasColumnName("xmin")
            .HasColumnType("xid")
            .IsRowVersion();

        builder.ToTable(table => table.HasCheckConstraint(
            "ck_user_accounts_locale",
            "locale IN ('zh-Hans', 'zh-Hant', 'en')"));
        builder.ToTable(table => table.HasCheckConstraint(
            "ck_user_accounts_status",
            "status IN (1, 2, 3, 4, 5)"));
    }
}
