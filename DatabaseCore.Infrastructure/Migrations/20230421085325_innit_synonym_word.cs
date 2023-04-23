using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCore.Infrastructure.Migrations
{
    public partial class innit_synonym_word : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Synonym",
                schema: "usr",
                table: "question",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Synonym",
                schema: "usr",
                table: "question");
        }
    }
}
