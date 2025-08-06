using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDepositTypeToPayoffs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payoffs_Invoices_InvoiceId",
                table: "Payoffs");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                table: "Payoffs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DepositType",
                table: "Payoffs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Payoffs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payoffs_ProjectId",
                table: "Payoffs",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payoffs_Invoices_InvoiceId",
                table: "Payoffs",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payoffs_Projects_ProjectId",
                table: "Payoffs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payoffs_Invoices_InvoiceId",
                table: "Payoffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Payoffs_Projects_ProjectId",
                table: "Payoffs");

            migrationBuilder.DropIndex(
                name: "IX_Payoffs_ProjectId",
                table: "Payoffs");

            migrationBuilder.DropColumn(
                name: "DepositType",
                table: "Payoffs");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Payoffs");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                table: "Payoffs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payoffs_Invoices_InvoiceId",
                table: "Payoffs",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
