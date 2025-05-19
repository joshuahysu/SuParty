using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuParty.Migrations
{
    /// <inheritdoc />
    public partial class a48484 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReferrerMembers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LeftId = table.Column<string>(type: "TEXT", nullable: true),
                    RightId = table.Column<string>(type: "TEXT", nullable: true),
                    SponsorId = table.Column<string>(type: "TEXT", nullable: true),
                    LeftPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    RightPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalEarnings = table.Column<int>(type: "INTEGER", nullable: false),
                    BonusLogs = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferrerMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferrerMembers_ReferrerMembers_LeftId",
                        column: x => x.LeftId,
                        principalTable: "ReferrerMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferrerMembers_ReferrerMembers_RightId",
                        column: x => x.RightId,
                        principalTable: "ReferrerMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferrerMembers_ReferrerMembers_SponsorId",
                        column: x => x.SponsorId,
                        principalTable: "ReferrerMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferrerMembers_LeftId",
                table: "ReferrerMembers",
                column: "LeftId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferrerMembers_RightId",
                table: "ReferrerMembers",
                column: "RightId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferrerMembers_SponsorId",
                table: "ReferrerMembers",
                column: "SponsorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferrerMembers");
        }
    }
}
