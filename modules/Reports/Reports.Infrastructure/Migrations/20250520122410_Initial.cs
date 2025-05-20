using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reports.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Customers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Customers", x => x.Id));

        migrationBuilder.CreateTable(
            name: "SalesReports",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ReportTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                GeneratedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                ReportPeriod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CustomerInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SalesReports", x => x.Id);
                table.ForeignKey(
                    name: "FK_SalesReports_Customers_CustomerInfoId",
                    column: x => x.CustomerInfoId,
                    principalTable: "Customers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "SaleDetails",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false),
                UnitPrice = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                SalesReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SaleDetails", x => x.Id);
                table.ForeignKey(
                    name: "FK_SaleDetails_SalesReports_SalesReportId",
                    column: x => x.SalesReportId,
                    principalTable: "SalesReports",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_SaleDetails_SalesReportId",
            table: "SaleDetails",
            column: "SalesReportId");

        migrationBuilder.CreateIndex(
            name: "IX_SalesReports_CustomerInfoId",
            table: "SalesReports",
            column: "CustomerInfoId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SaleDetails");

        migrationBuilder.DropTable(
            name: "SalesReports");

        migrationBuilder.DropTable(
            name: "Customers");
    }
}
