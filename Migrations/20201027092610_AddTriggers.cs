using Microsoft.EntityFrameworkCore.Migrations;

namespace Venjix.Migrations
{
    public partial class AddTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Triggers",
                columns: table => new
                {
                    TriggerId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Event = table.Column<int>(nullable: false),
                    Value = table.Column<double>(nullable: false),
                    SendOnFirstMatch = table.Column<bool>(nullable: false),
                    HookEnabled = table.Column<bool>(nullable: false),
                    SensorId = table.Column<int>(nullable: false),
                    TelegramEnabled = table.Column<bool>(nullable: false),
                    TelegramId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triggers", x => x.TriggerId);
                    table.ForeignKey(
                        name: "FK_Triggers_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "SensorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Triggers_Telegrams_TelegramId",
                        column: x => x.TelegramId,
                        principalTable: "Telegrams",
                        principalColumn: "TelegramId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_SensorId",
                table: "Triggers",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_TelegramId",
                table: "Triggers",
                column: "TelegramId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Triggers");
        }
    }
}
