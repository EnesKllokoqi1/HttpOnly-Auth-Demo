using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookieAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedExtraProperties : Migration
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

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "User",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
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
