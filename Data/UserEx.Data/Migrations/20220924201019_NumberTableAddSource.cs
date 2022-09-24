using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserEx.Data.Migrations
{
    public partial class NumberTableAddSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "Numbers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "Numbers");
        }
    }
}
