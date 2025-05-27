using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuParty.Migrations
{
    /// <inheritdoc />
    public partial class aasf22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MonthSpend",
                table: "ReferrerMembers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "HouseDatas",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IsPublic",
                table: "HouseDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthSpend",
                table: "ReferrerMembers");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "HouseDatas");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "HouseDatas");
        }
    }
}
