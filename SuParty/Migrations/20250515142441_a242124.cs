using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuParty.Migrations
{
    /// <inheritdoc />
    public partial class a242124 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalFloor",
                table: "HouseDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalFloor",
                table: "HouseDatas");
        }
    }
}
