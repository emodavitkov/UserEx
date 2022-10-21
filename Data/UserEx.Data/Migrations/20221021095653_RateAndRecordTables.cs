#nullable disable

namespace UserEx.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RateAndRecordTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    DialCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderId = table.Column<int>(type: "int", nullable: false),
                    DestinationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => new { x.ProviderId, x.DialCode });
                    table.ForeignKey(
                        name: "FK_Rates_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CallerNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CallingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuyRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    ProviderId = table.Column<int>(type: "int", nullable: false),
                    DialCode = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Records_Rates_ProviderId_DialCode",
                        columns: x => new { x.ProviderId, x.DialCode },
                        principalTable: "Rates",
                        principalColumns: new[] { "ProviderId", "DialCode" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_ProviderId_DialCode",
                table: "Records",
                columns: new[] { "ProviderId", "DialCode" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "Rates");
        }
    }
}
