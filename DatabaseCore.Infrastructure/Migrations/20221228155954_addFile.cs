using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCore.Infrastructure.Migrations
{
    public partial class addFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Added",
                table: "QuestionStandards",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Added",
                table: "QuestionStandards");
        }
    }
}
