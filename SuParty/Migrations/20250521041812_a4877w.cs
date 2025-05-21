using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuParty.Migrations
{
    /// <inheritdoc />
    public partial class a4877w : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Trackings",
                table: "Trackings");

            migrationBuilder.DropColumn(
                name: "TraceRealEstates",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "TrackingList",
                table: "Trackings");

            migrationBuilder.RenameTable(
                name: "Trackings",
                newName: "Tracking");

            migrationBuilder.AddColumn<int>(
                name: "LoveScore",
                table: "Tracking",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tracking",
                table: "Tracking",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TrackingObject",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    LoveScore = table.Column<int>(type: "INTEGER", nullable: false),
                    RealEstateUserDataId = table.Column<string>(type: "TEXT", nullable: true),
                    TrackingId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingObject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackingObject_Tracking_TrackingId",
                        column: x => x.TrackingId,
                        principalTable: "Tracking",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrackingObject_UserData_RealEstateUserDataId",
                        column: x => x.RealEstateUserDataId,
                        principalTable: "UserData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackingObject_RealEstateUserDataId",
                table: "TrackingObject",
                column: "RealEstateUserDataId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingObject_TrackingId",
                table: "TrackingObject",
                column: "TrackingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackingObject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tracking",
                table: "Tracking");

            migrationBuilder.DropColumn(
                name: "LoveScore",
                table: "Tracking");

            migrationBuilder.RenameTable(
                name: "Tracking",
                newName: "Trackings");

            migrationBuilder.AddColumn<string>(
                name: "TraceRealEstates",
                table: "UserData",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingList",
                table: "Trackings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trackings",
                table: "Trackings",
                column: "Id");
        }
    }
}
