using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCore.Infrastructure.Migrations
{
    public partial class update_code_CoursTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "usr",
                table: "Cours",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "usr",
                table: "Cours");
        }
    }
}
