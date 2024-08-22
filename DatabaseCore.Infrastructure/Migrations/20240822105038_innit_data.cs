using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DatabaseCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class innit_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "usr");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "usr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fk_userid = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    define = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    totalquestion = table.Column<int>(type: "integer", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cours",
                schema: "usr",
                columns: table => new
                {
                    Pk_coursId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Target = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    CoursImage = table.Column<string>(type: "text", nullable: true),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cours", x => x.Pk_coursId);
                });

            migrationBuilder.CreateTable(
                name: "Grammar",
                schema: "usr",
                columns: table => new
                {
                    PK_grammarId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FK_UserId = table.Column<int>(type: "integer", nullable: false),
                    grammar_name = table.Column<string>(type: "text", nullable: true),
                    grammar_content = table.Column<string>(type: "text", nullable: true),
                    Concept = table.Column<string>(type: "text", nullable: true),
                    created_on = table.Column<DateTime>(type: "timestamp", nullable: false),
                    KD = table.Column<string>(type: "text", nullable: true),
                    PD = table.Column<string>(type: "text", nullable: true),
                    NV = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grammar", x => x.PK_grammarId);
                });

            migrationBuilder.CreateTable(
                name: "News",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "usr",
                columns: table => new
                {
                    Pk_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FK_UserId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Pk_Id);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                schema: "usr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PermissionKey = table.Column<string>(type: "text", nullable: true),
                    PermissionName = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "phrase",
                schema: "usr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fk_readingId = table.Column<int>(type: "integer", nullable: false),
                    word = table.Column<string>(type: "text", nullable: true),
                    definition = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phrase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "question",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fk_userid = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    questionname = table.Column<string>(type: "text", nullable: true),
                    answer = table.Column<string>(type: "text", nullable: true),
                    ImageSource = table.Column<string>(type: "text", nullable: true),
                    phonetic = table.Column<string>(type: "text", nullable: true),
                    audio = table.Column<string>(type: "text", nullable: true),
                    created_on = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_delete = table.Column<int>(type: "integer", nullable: false),
                    definition = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Synonym = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question", x => x.id);
                });

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
                    Note = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Added = table.Column<bool>(type: "boolean", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Audio = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionStandards", x => x.Pk_QuestionStandard_Id);
                });

            migrationBuilder.CreateTable(
                name: "Reading",
                schema: "usr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Definetion = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Translate = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    TaskName = table.Column<string>(type: "text", nullable: true),
                    Band = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reading", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReadingQuestion",
                schema: "usr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fk_reding_id = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Answer = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingQuestion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "report",
                schema: "usr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FK_UserId = table.Column<int>(type: "integer", nullable: false),
                    TotalWords = table.Column<int>(type: "integer", nullable: false),
                    TotalPosts = table.Column<int>(type: "integer", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "usr",
                columns: table => new
                {
                    PK_RoleID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.PK_RoleID);
                });

            migrationBuilder.CreateTable(
                name: "rolepermission",
                schema: "usr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FK_PermissionID = table.Column<int>(type: "integer", nullable: false),
                    FK_RoleID = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rolepermission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Target",
                schema: "usr",
                columns: table => new
                {
                    PK_TargetId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FK_UserId = table.Column<int>(type: "integer", nullable: false),
                    target_content = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    total_days = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Target", x => x.PK_TargetId);
                });

            migrationBuilder.CreateTable(
                name: "topic",
                schema: "usr",
                columns: table => new
                {
                    PK_Topic_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FK_Course_Id = table.Column<int>(type: "integer", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    EnglishContent = table.Column<string>(type: "text", nullable: true),
                    VietNamContent = table.Column<string>(type: "text", nullable: true),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topic", x => x.PK_Topic_Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "usr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "varchar", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "varchar", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar", maxLength: 20, nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    DeviceToken = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    isnotification = table.Column<int>(type: "integer", nullable: false),
                    Avartar = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserManual",
                schema: "usr",
                columns: table => new
                {
                    Pk_UserManual_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fk_Grannar_Id = table.Column<int>(type: "integer", nullable: false),
                    Used = table.Column<string>(type: "text", nullable: true),
                    Example = table.Column<string>(type: "text", nullable: true),
                    DetailExample = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserManual", x => x.Pk_UserManual_Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "usr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FK_RoleID = table.Column<int>(type: "integer", nullable: false),
                    FK_UserID = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStandardQuestion",
                schema: "usr",
                columns: table => new
                {
                    StandardQuestion = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrimaryKey_QuestionStandardAndUser", x => new { x.StandardQuestion, x.UserId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grammar_Code",
                schema: "usr",
                table: "Grammar",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "Cours",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "Grammar",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "News",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "permission",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "phrase",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "product",
                schema: "public");

            migrationBuilder.DropTable(
                name: "question",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "QuestionStandardDetail",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "QuestionStandards");

            migrationBuilder.DropTable(
                name: "Reading",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "ReadingQuestion",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "report",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "rolepermission",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "Target",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "topic",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "User",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "UserManual",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "UserStandardQuestion",
                schema: "usr");
        }
    }
}
