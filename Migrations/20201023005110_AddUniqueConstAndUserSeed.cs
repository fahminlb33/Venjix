using Microsoft.EntityFrameworkCore.Migrations;

namespace Venjix.Migrations
{
    public partial class AddUniqueConstAndUserSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Password", "Role", "Username" },
                values: new object[] { 1, "$2y$12$XIMeV8tAOoC7D0XRJF5TjOQNy0T9Wj71JkETAdEmrjH6X9nIf50ZO", "Admin", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
