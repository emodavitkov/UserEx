using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserEx.Data.Migrations
{
    public partial class RecordsToNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberId",
                table: "Records",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Records_NumberId",
                table: "Records",
                column: "NumberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Numbers_NumberId",
                table: "Records",
                column: "NumberId",
                principalTable: "Numbers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Numbers_NumberId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_NumberId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "NumberId",
                table: "Records");
        }
    }
}
