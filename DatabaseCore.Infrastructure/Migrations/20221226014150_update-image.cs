using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCore.Infrastructure.Migrations
{
    public partial class updateimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoursImage",
                schema: "usr",
                table: "Cours",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoursImage",
                schema: "usr",
                table: "Cours");
        }
    }
}
