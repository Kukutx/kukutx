using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Platform.Infrastructure.Persistence.Migrations;

[DbContext(typeof(PlatformDbContext))]
public sealed class PlatformDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.9")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        modelBuilder.Entity("Platform.Domain.Identity.UserAccount", builder =>
        {
            builder.Property<Guid>("Id").HasColumnType("uuid").HasColumnName("id");
            builder.Property<Guid?>("CityId").HasColumnType("uuid").HasColumnName("city_id");
            builder.Property<DateTimeOffset>("CreatedAt").HasColumnType("timestamp with time zone").HasColumnName("created_at");
            builder.Property<string>("DisplayName").HasMaxLength(80).HasColumnType("character varying(80)").HasColumnName("display_name");
            builder.Property<string>("Email").HasMaxLength(320).HasColumnType("character varying(320)").HasColumnName("email");
            builder.Property<string>("FirebaseUid").IsRequired().HasMaxLength(128).HasColumnType("character varying(128)").HasColumnName("firebase_uid");
            builder.Property<string>("Locale").IsRequired().HasMaxLength(16).HasColumnType("character varying(16)").HasColumnName("locale");
            builder.Property<short>("Status").HasColumnType("smallint").HasColumnName("status");
            builder.Property<string>("TimeZoneId").IsRequired().HasMaxLength(64).HasColumnType("character varying(64)").HasColumnName("time_zone_id");
            builder.Property<DateTimeOffset>("UpdatedAt").HasColumnType("timestamp with time zone").HasColumnName("updated_at");
            builder.Property<uint>("Version").IsConcurrencyToken().ValueGeneratedOnAddOrUpdate().HasColumnType("xid").HasColumnName("xmin");

            builder.HasKey("Id");
            builder.HasIndex("FirebaseUid").IsUnique().HasDatabaseName("ux_user_accounts_firebase_uid");
            builder.ToTable("user_accounts", tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("ck_user_accounts_locale", "locale IN ('zh-Hans', 'zh-Hant', 'en')");
                tableBuilder.HasCheckConstraint("ck_user_accounts_status", "status IN (1, 2, 3, 4, 5)");
            });
        });
    }
}
