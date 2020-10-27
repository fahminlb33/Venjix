using Microsoft.EntityFrameworkCore.Migrations;

namespace Venjix.Migrations
{
    public partial class AddWebhooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Webhooks",
                columns: table => new
                {
                    WebhookId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    UriFormat = table.Column<string>(nullable: true),
                    BodyFormat = table.Column<string>(nullable: true),
                    JsonBody = table.Column<bool>(nullable: false),
                    Method = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Webhooks", x => x.WebhookId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Webhooks");
        }
    }
}
