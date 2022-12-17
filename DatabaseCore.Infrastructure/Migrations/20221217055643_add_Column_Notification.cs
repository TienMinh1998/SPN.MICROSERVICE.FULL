using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCore.Infrastructure.Migrations
{
    public partial class add_Column_Notification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification",
                newSchema: "usr");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                schema: "usr",
                table: "Notification",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                schema: "usr",
                table: "Notification",
                column: "Pk_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                schema: "usr",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "Notification",
                schema: "usr",
                newName: "Notifications");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_on",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Pk_Id");
        }
    }
}
