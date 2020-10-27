using Microsoft.EntityFrameworkCore.Migrations;

namespace Venjix.Migrations
{
    public partial class TriggerAddNameField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Triggers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Triggers");
        }
    }
}
