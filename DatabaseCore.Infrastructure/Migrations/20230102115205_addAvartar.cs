using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCore.Infrastructure.Migrations
{
    public partial class addAvartar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avartar",
                schema: "usr",
                table: "User",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avartar",
                schema: "usr",
                table: "User");
        }
    }
}
