using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAlbum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlbumId",
                table: "UploadedFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFiles_AlbumId",
                table: "UploadedFiles",
                column: "AlbumId");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadedFiles_Albums_AlbumId",
                table: "UploadedFiles",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadedFiles_Albums_AlbumId",
                table: "UploadedFiles");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_UploadedFiles_AlbumId",
                table: "UploadedFiles");

            migrationBuilder.DropColumn(
                name: "AlbumId",
                table: "UploadedFiles");
        }
    }
}
