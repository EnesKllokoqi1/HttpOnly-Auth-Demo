using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookieAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedAgeProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<char>(
                name: "Gender",
                table: "User",
                type: "character(1)",
                nullable: true,
                oldClrType: typeof(char),
                oldType: "character(1)");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "User",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "User");

            migrationBuilder.AlterColumn<char>(
                name: "Gender",
                table: "User",
                type: "character(1)",
                nullable: false,
                defaultValue: '\0',
                oldClrType: typeof(char),
                oldType: "character(1)",
                oldNullable: true);
        }
    }
}
