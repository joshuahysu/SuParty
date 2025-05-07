using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuParty.Migrations
{
    /// <inheritdoc />
    public partial class aaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "ProductDatas",
                newName: "Images");

            migrationBuilder.AddColumn<string>(
                name: "SalesId",
                table: "ProductDatas",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesId",
                table: "ProductDatas");

            migrationBuilder.RenameColumn(
                name: "Images",
                table: "ProductDatas",
                newName: "Image");
        }
    }
}
