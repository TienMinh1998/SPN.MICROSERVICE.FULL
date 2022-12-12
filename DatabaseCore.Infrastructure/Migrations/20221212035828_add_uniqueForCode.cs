using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCore.Infrastructure.Migrations
{
    public partial class add_uniqueForCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "usr",
                table: "Grammar",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grammar_Code",
                schema: "usr",
                table: "Grammar",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Grammar_Code",
                schema: "usr",
                table: "Grammar");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "usr",
                table: "Grammar");
        }
    }
}
