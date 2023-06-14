using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfUPWeb.Migrations
{
    /// <inheritdoc />
    public partial class MVCSurUpBoards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurfBoards",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    LengthFeet = table.Column<double>(type: "float", nullable: false),
                    LengthInch = table.Column<double>(type: "float", nullable: false),
                    Thicc = table.Column<double>(type: "float", nullable: false),
                    Volume = table.Column<double>(type: "float", nullable: false),
                    Exstra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_public_Id = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfBoards", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurfBoards");
        }
    }
}
