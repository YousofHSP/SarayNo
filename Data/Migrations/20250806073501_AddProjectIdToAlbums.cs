using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectIdToAlbums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ProjectId",
                table: "Albums",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Projects_ProjectId",
                table: "Albums",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Projects_ProjectId",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Albums_ProjectId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Albums");
        }
    }
}
