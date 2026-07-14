using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Platform.Infrastructure.Persistence.Migrations;

[DbContext(typeof(PlatformDbContext))]
[Migration("202607140001_InitialIdentity")]
public sealed class InitialIdentity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "user_accounts",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                firebase_uid = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                display_name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                locale = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                time_zone_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                city_id = table.Column<Guid>(type: "uuid", nullable: true),
                status = table.Column<short>(type: "smallint", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_user_accounts", candidate => candidate.id);
                table.CheckConstraint("ck_user_accounts_locale", "locale IN ('zh-Hans', 'zh-Hant', 'en')");
                table.CheckConstraint("ck_user_accounts_status", "status IN (1, 2, 3, 4, 5)");
            });

        migrationBuilder.CreateIndex(
            name: "ux_user_accounts_firebase_uid",
            table: "user_accounts",
            column: "firebase_uid",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "user_accounts");
    }
}
