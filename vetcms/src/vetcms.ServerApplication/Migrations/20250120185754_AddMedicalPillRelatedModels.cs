using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vetcms.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicalPillRelatedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalPills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Dosage = table.Column<string>(type: "TEXT", nullable: false),
                    Form = table.Column<string>(type: "TEXT", nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    ActiveIngredient = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalPills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalPillStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PillId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StorageLocation = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalPillStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalPillStock_MedicalPills_PillId",
                        column: x => x.PillId,
                        principalTable: "MedicalPills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalPillStockAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PillId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    GeneratedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalPillStockAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalPillStockAlerts_MedicalPills_PillId",
                        column: x => x.PillId,
                        principalTable: "MedicalPills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalPillUsageLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PillId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantityUsed = table.Column<int>(type: "INTEGER", nullable: false),
                    UsageDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Purpose = table.Column<string>(type: "TEXT", nullable: false),
                    UsedById = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalPillUsageLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalPillUsageLogs_MedicalPills_PillId",
                        column: x => x.PillId,
                        principalTable: "MedicalPills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalPillUsageLogs_Users_UsedById",
                        column: x => x.UsedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalPillStock_PillId",
                table: "MedicalPillStock",
                column: "PillId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalPillStockAlerts_PillId",
                table: "MedicalPillStockAlerts",
                column: "PillId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalPillUsageLogs_PillId",
                table: "MedicalPillUsageLogs",
                column: "PillId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalPillUsageLogs_UsedById",
                table: "MedicalPillUsageLogs",
                column: "UsedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalPillStock");

            migrationBuilder.DropTable(
                name: "MedicalPillStockAlerts");

            migrationBuilder.DropTable(
                name: "MedicalPillUsageLogs");

            migrationBuilder.DropTable(
                name: "MedicalPills");
        }
    }
}
