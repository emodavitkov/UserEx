#nullable disable

namespace UserEx.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RecordsToNumbersPropertyChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CallerNumber",
                table: "Records");

            migrationBuilder.AddColumn<string>(
                name: "CallerNumberNotProcured",
                table: "Records",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CallerNumberNotProcured",
                table: "Records");

            migrationBuilder.AddColumn<string>(
                name: "CallerNumber",
                table: "Records",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
