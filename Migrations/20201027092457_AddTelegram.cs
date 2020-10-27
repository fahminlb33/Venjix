using Microsoft.EntityFrameworkCore.Migrations;

namespace Venjix.Migrations
{
    public partial class AddTelegram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Telegrams",
                columns: table => new
                {
                    TelegramId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageFormat = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telegrams", x => x.TelegramId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Telegrams");
        }
    }
}
