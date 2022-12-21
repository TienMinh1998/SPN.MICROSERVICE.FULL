using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCore.Infrastructure.Migrations
{
    public partial class innit_QuestionStandardDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionStandardDetail",
                schema: "usr",
                columns: table => new
                {
                    QuestionID = table.Column<int>(type: "integer", nullable: false),
                    TopicID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_QuestionStandardAndTopic", x => new { x.QuestionID, x.TopicID });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionStandardDetail",
                schema: "usr");
        }
    }
}
