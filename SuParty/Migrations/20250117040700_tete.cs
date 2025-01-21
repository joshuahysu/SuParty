using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuParty.Migrations
{
    /// <inheritdoc />
    public partial class tete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "HouseDatas",
                newName: "Images");

            migrationBuilder.AddColumn<int>(
                name: "HouseType",
                table: "HouseDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaintenanceFee",
                table: "HouseDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParkingSpaceType",
                table: "HouseDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HouseType",
                table: "HouseDatas");

            migrationBuilder.DropColumn(
                name: "MaintenanceFee",
                table: "HouseDatas");

            migrationBuilder.DropColumn(
                name: "ParkingSpaceType",
                table: "HouseDatas");

            migrationBuilder.RenameColumn(
                name: "Images",
                table: "HouseDatas",
                newName: "Image");
        }
    }
}
