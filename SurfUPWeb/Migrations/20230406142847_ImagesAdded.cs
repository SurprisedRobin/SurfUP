using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfUPWeb.Migrations
{
    /// <inheritdoc />
    public partial class ImagesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MyProperty",
                table: "MyProperty");

            migrationBuilder.RenameTable(
                name: "MyProperty",
                newName: "SurfBoards");

            migrationBuilder.RenameColumn(
                name: "Length",
                table: "SurfBoards",
                newName: "Width");

            migrationBuilder.RenameColumn(
                name: "Hight",
                table: "SurfBoards",
                newName: "LengthInch");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "SurfBoards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "LengthFeet",
                table: "SurfBoards",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurfBoards",
                table: "SurfBoards",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SurfBoards",
                table: "SurfBoards");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "SurfBoards");

            migrationBuilder.DropColumn(
                name: "LengthFeet",
                table: "SurfBoards");

            migrationBuilder.RenameTable(
                name: "SurfBoards",
                newName: "MyProperty");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "MyProperty",
                newName: "Length");

            migrationBuilder.RenameColumn(
                name: "LengthInch",
                table: "MyProperty",
                newName: "Hight");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyProperty",
                table: "MyProperty",
                column: "ID");
        }
    }
}
