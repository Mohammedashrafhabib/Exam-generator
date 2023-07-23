using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGService.DataAccess.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UserManagement");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "UserManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    FirstModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifiedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    MustDeletedPhysical = table.Column<bool>(type: "bit", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "UserManagement",
                table: "Users",
                columns: new[] { "Id", "CreatedByUserId", "CreationDate", "DeletedByUserId", "DeletionDate", "Email", "FirstModificationDate", "FirstModifiedByUserId", "IsActive", "IsDeleted", "LastModificationDate", "LastModifiedByUserId", "MustDeletedPhysical", "NameAr", "NameEn", "Password", "PhoneNumber", "Username" },
                values: new object[] { 1L, null, new DateTime(2022, 9, 8, 13, 15, 24, 581, DateTimeKind.Local), null, null, null, null, null, true, false, null, null, null, "مدير النظام", "admin", "ANRY6T1hsNiVIMyBzkFP7iBPFPynnAZLDXtXJcHXR9Xj6qwVS7+WJwrvEIvH1vTCtA==", null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_NameEn",
                schema: "UserManagement",
                table: "Users",
                column: "NameEn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "UserManagement",
                table: "Users",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "UserManagement");
        }
    }
}
