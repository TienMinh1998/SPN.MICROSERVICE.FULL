using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DatabaseCore.Infrastructure.Migrations
{
    public partial class initTable_QuestionStandard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionStandards",
                columns: table => new
                {
                    Pk_QuestionStandard_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    English = table.Column<string>(type: "text", nullable: true),
                    Phonetic = table.Column<string>(type: "text", nullable: true),
                    MeaningEnglish = table.Column<string>(type: "text", nullable: true),
                    MeaningVietNam = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionStandards", x => x.Pk_QuestionStandard_Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionStandards");
        }
    }
}
