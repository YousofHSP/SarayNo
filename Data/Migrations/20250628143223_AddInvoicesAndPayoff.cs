using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoicesAndPayoff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "Activities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UnverifiedInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    CostGroupId = table.Column<int>(type: "int", nullable: false),
                    CreditorId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnverifiedInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnverifiedInvoices_CostGroups_CostGroupId",
                        column: x => x.CostGroupId,
                        principalTable: "CostGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnverifiedInvoices_Creditors_CreditorId",
                        column: x => x.CreditorId,
                        principalTable: "Creditors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnverifiedInvoices_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnsettledInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: true),
                    UnverifiedInvoiceId = table.Column<int>(type: "int", nullable: true),
                    CostGroupId = table.Column<int>(type: "int", nullable: false),
                    CreditorId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RemainAmount = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnsettledInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnsettledInvoices_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UnsettledInvoices_CostGroups_CostGroupId",
                        column: x => x.CostGroupId,
                        principalTable: "CostGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnsettledInvoices_Creditors_CreditorId",
                        column: x => x.CreditorId,
                        principalTable: "Creditors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnsettledInvoices_UnverifiedInvoices_UnverifiedInvoiceId",
                        column: x => x.UnverifiedInvoiceId,
                        principalTable: "UnverifiedInvoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payoffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnsettledInvoiceId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payoffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payoffs_UnsettledInvoices_UnsettledInvoiceId",
                        column: x => x.UnsettledInvoiceId,
                        principalTable: "UnsettledInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payoffs_UnsettledInvoiceId",
                table: "Payoffs",
                column: "UnsettledInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UnsettledInvoices_ActivityId",
                table: "UnsettledInvoices",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UnsettledInvoices_CostGroupId",
                table: "UnsettledInvoices",
                column: "CostGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UnsettledInvoices_CreditorId",
                table: "UnsettledInvoices",
                column: "CreditorId");

            migrationBuilder.CreateIndex(
                name: "IX_UnsettledInvoices_UnverifiedInvoiceId",
                table: "UnsettledInvoices",
                column: "UnverifiedInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UnverifiedInvoices_CostGroupId",
                table: "UnverifiedInvoices",
                column: "CostGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UnverifiedInvoices_CreditorId",
                table: "UnverifiedInvoices",
                column: "CreditorId");

            migrationBuilder.CreateIndex(
                name: "IX_UnverifiedInvoices_ProjectId",
                table: "UnverifiedInvoices",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payoffs");

            migrationBuilder.DropTable(
                name: "UnsettledInvoices");

            migrationBuilder.DropTable(
                name: "UnverifiedInvoices");

            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "Activities");
        }
    }
}
