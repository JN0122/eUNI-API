using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eUNI_API.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldOfStudies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Abbr = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StudiesCycle = table.Column<byte>(type: "smallint", nullable: false),
                    SemesterCount = table.Column<byte>(type: "smallint", nullable: false),
                    IsFullTime = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOfStudies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Abbr = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartHour = table.Column<byte>(type: "smallint", nullable: false),
                    StartMinute = table.Column<byte>(type: "smallint", nullable: false),
                    EndHour = table.Column<byte>(type: "smallint", nullable: false),
                    EndMinute = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Years",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Years", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    LastName = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Salt = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationsOfTheYear",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    YearId = table.Column<int>(type: "integer", nullable: false),
                    FirstHalfOfYear = table.Column<bool>(type: "boolean", nullable: false),
                    StartDay = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDay = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationsOfTheYear", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationsOfTheYear_Years_YearId",
                        column: x => x.YearId,
                        principalTable: "Years",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DaysOff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizationsOfTheYearId = table.Column<int>(type: "integer", nullable: false),
                    Day = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysOff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaysOff_OrganizationsOfTheYear_OrganizationsOfTheYearId",
                        column: x => x.OrganizationsOfTheYearId,
                        principalTable: "OrganizationsOfTheYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldOfStudyLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FieldOfStudyId = table.Column<int>(type: "integer", nullable: false),
                    OrganizationsOfTheYearId = table.Column<int>(type: "integer", nullable: false),
                    Semester = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOfStudyLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldOfStudyLogs_FieldOfStudies_FieldOfStudyId",
                        column: x => x.FieldOfStudyId,
                        principalTable: "FieldOfStudies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldOfStudyLogs_OrganizationsOfTheYear_OrganizationsOfTheY~",
                        column: x => x.OrganizationsOfTheYearId,
                        principalTable: "OrganizationsOfTheYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FieldOfStudyLogId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Room = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    StartHourId = table.Column<int>(type: "integer", nullable: false),
                    EndHourId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_FieldOfStudyLogs_FieldOfStudyLogId",
                        column: x => x.FieldOfStudyLogId,
                        principalTable: "FieldOfStudyLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_Hours_EndHourId",
                        column: x => x.EndHourId,
                        principalTable: "Hours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_Hours_StartHourId",
                        column: x => x.StartHourId,
                        principalTable: "Hours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentFieldsOfStudyLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FieldsOfStudyLogId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsRepresentative = table.Column<bool>(type: "boolean", nullable: false),
                    IsCurrentFieldOfStudy = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFieldsOfStudyLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentFieldsOfStudyLogs_FieldOfStudyLogs_FieldsOfStudyLogId",
                        column: x => x.FieldsOfStudyLogId,
                        principalTable: "FieldOfStudyLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentFieldsOfStudyLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassDates_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentsFieldsOfStudyLogId = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentGroups_StudentFieldsOfStudyLogs_StudentsFieldsOfStud~",
                        column: x => x.StudentsFieldsOfStudyLogId,
                        principalTable: "StudentFieldsOfStudyLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "Abbr", "Type" },
                values: new object[,]
                {
                    { 1, "P01", 2 },
                    { 2, "P02", 2 },
                    { 3, "P03", 2 },
                    { 4, "P04", 2 },
                    { 5, "P05", 2 },
                    { 6, "W", 0 },
                    { 7, "K01", 3 },
                    { 8, "K02", 3 },
                    { 9, "K03", 3 },
                    { 10, "K04", 3 },
                    { 11, "K05", 3 },
                    { 12, "L01", 4 },
                    { 13, "L02", 4 },
                    { 14, "L03", 4 },
                    { 15, "L04", 4 },
                    { 16, "L05", 4 },
                    { 17, "1", 1 },
                    { 18, "2", 1 },
                    { 19, "3", 1 },
                    { 20, "4", 1 }
                });

            migrationBuilder.InsertData(
                table: "Hours",
                columns: new[] { "Id", "EndHour", "EndMinute", "StartHour", "StartMinute" },
                values: new object[,]
                {
                    { 1, (byte)8, (byte)15, (byte)7, (byte)30 },
                    { 2, (byte)9, (byte)0, (byte)8, (byte)15 },
                    { 3, (byte)10, (byte)0, (byte)9, (byte)15 },
                    { 4, (byte)10, (byte)45, (byte)10, (byte)0 },
                    { 5, (byte)11, (byte)45, (byte)11, (byte)0 },
                    { 6, (byte)12, (byte)30, (byte)11, (byte)45 },
                    { 7, (byte)13, (byte)30, (byte)12, (byte)45 },
                    { 8, (byte)14, (byte)15, (byte)13, (byte)30 },
                    { 9, (byte)15, (byte)15, (byte)14, (byte)30 },
                    { 10, (byte)16, (byte)0, (byte)15, (byte)15 },
                    { 11, (byte)17, (byte)0, (byte)16, (byte)15 },
                    { 12, (byte)17, (byte)45, (byte)17, (byte)0 },
                    { 13, (byte)18, (byte)45, (byte)18, (byte)0 },
                    { 14, (byte)19, (byte)30, (byte)18, (byte)45 },
                    { 15, (byte)20, (byte)30, (byte)19, (byte)45 },
                    { 16, (byte)21, (byte)15, (byte)20, (byte)30 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Student" }
                });

            migrationBuilder.InsertData(
                table: "Years",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "2024/2025" });

            migrationBuilder.InsertData(
                table: "OrganizationsOfTheYear",
                columns: new[] { "Id", "EndDay", "FirstHalfOfYear", "StartDay", "YearId" },
                values: new object[] { 1, new DateOnly(2025, 1, 26), true, new DateOnly(2024, 10, 1), 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsDeleted", "LastName", "PasswordHash", "RoleId", "Salt" },
                values: new object[] { new Guid("c49c2319-2e87-45fe-be3b-1d9e724df781"), new DateTime(2024, 12, 12, 23, 37, 26, 169, DateTimeKind.Utc).AddTicks(4690), "root@euni.com", "Jan", false, "Kowalski", "NTWxiNrLLLT2HkXuG9JiPYN0z5UN2eHW5gMsxbP4ATY=", 1, "mwmeU7TZlMdR/NMWAJMzrQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_ClassDates_ClassId",
                table: "ClassDates",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_EndHourId",
                table: "Classes",
                column: "EndHourId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_FieldOfStudyLogId",
                table: "Classes",
                column: "FieldOfStudyLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_GroupId",
                table: "Classes",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_StartHourId",
                table: "Classes",
                column: "StartHourId");

            migrationBuilder.CreateIndex(
                name: "IX_DaysOff_OrganizationsOfTheYearId",
                table: "DaysOff",
                column: "OrganizationsOfTheYearId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOfStudyLogs_FieldOfStudyId",
                table: "FieldOfStudyLogs",
                column: "FieldOfStudyId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOfStudyLogs_OrganizationsOfTheYearId",
                table: "FieldOfStudyLogs",
                column: "OrganizationsOfTheYearId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationsOfTheYear_YearId",
                table: "OrganizationsOfTheYear",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetLogs_Token",
                table: "PasswordResetLogs",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetLogs_UserId",
                table: "PasswordResetLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFieldsOfStudyLogs_FieldsOfStudyLogId",
                table: "StudentFieldsOfStudyLogs",
                column: "FieldsOfStudyLogId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFieldsOfStudyLogs_UserId",
                table: "StudentFieldsOfStudyLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_GroupId",
                table: "StudentGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_StudentsFieldsOfStudyLogId",
                table: "StudentGroups",
                column: "StudentsFieldsOfStudyLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassDates");

            migrationBuilder.DropTable(
                name: "DaysOff");

            migrationBuilder.DropTable(
                name: "PasswordResetLogs");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "StudentGroups");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "StudentFieldsOfStudyLogs");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Hours");

            migrationBuilder.DropTable(
                name: "FieldOfStudyLogs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "FieldOfStudies");

            migrationBuilder.DropTable(
                name: "OrganizationsOfTheYear");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Years");
        }
    }
}
