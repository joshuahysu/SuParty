using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuParty.Migrations
{
    /// <inheritdoc />
    public partial class iii : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Line_Url",
                table: "UserDatas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TraceRealEstates",
                table: "UserDatas",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<int>(
                name: "LivingRoomCount",
                table: "HouseDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParkingSpace",
                table: "HouseDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParkingSpaceCount",
                table: "HouseDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RestroomCount",
                table: "HouseDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Line_Url",
                table: "UserDatas");

            migrationBuilder.DropColumn(
                name: "TraceRealEstates",
                table: "UserDatas");

            migrationBuilder.DropColumn(
                name: "LivingRoomCount",
                table: "HouseDatas");

            migrationBuilder.DropColumn(
                name: "ParkingSpace",
                table: "HouseDatas");

            migrationBuilder.DropColumn(
                name: "ParkingSpaceCount",
                table: "HouseDatas");

            migrationBuilder.DropColumn(
                name: "RestroomCount",
                table: "HouseDatas");
        }
    }
}
