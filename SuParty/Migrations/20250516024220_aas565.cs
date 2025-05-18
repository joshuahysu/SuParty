using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuParty.Migrations
{
    /// <inheritdoc />
    public partial class aas565 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShoppingCart",
                table: "UserData");

            migrationBuilder.AddColumn<decimal>(
                name: "ProfitSharing",
                table: "HouseDatas",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ProductDataUserData",
                columns: table => new
                {
                    ShoppingCartId = table.Column<string>(type: "TEXT", nullable: false),
                    UsersId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDataUserData", x => new { x.ShoppingCartId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ProductDataUserData_ProductDatas_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ProductDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDataUserData_UserData_UsersId",
                        column: x => x.UsersId,
                        principalTable: "UserData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDataUserData_UsersId",
                table: "ProductDataUserData",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDataUserData");

            migrationBuilder.DropColumn(
                name: "ProfitSharing",
                table: "HouseDatas");

            migrationBuilder.AddColumn<string>(
                name: "ShoppingCart",
                table: "UserData",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
