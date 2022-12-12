using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCore.Infrastructure.Migrations
{
    public partial class add_new_column_grammartable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KD",
                schema: "usr",
                table: "Grammar",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NV",
                schema: "usr",
                table: "Grammar",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PD",
                schema: "usr",
                table: "Grammar",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KD",
                schema: "usr",
                table: "Grammar");

            migrationBuilder.DropColumn(
                name: "NV",
                schema: "usr",
                table: "Grammar");

            migrationBuilder.DropColumn(
                name: "PD",
                schema: "usr",
                table: "Grammar");
        }
    }
}
