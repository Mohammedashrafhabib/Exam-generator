using Microsoft.EntityFrameworkCore.Migrations;

namespace EGService.DataAccess.Migrations
{
    public partial class fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameEn",
                schema: "UserManagement",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "NameAr",
                schema: "UserManagement",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameIndex(
                name: "IX_Users_NameEn",
                schema: "UserManagement",
                table: "Users",
                newName: "IX_Users_LastName");

            migrationBuilder.UpdateData(
                schema: "UserManagement",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "FirstName", "Password" },
                values: new object[] { "admin", "AI2LIgMF+0Or7FUWah+Gt5G2CZVmpQhbRdUz4RWqtnXjHhNM1R+00TT0QlmG+NcVyQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                schema: "UserManagement",
                table: "Users",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                schema: "UserManagement",
                table: "Users",
                newName: "NameAr");

            migrationBuilder.RenameIndex(
                name: "IX_Users_LastName",
                schema: "UserManagement",
                table: "Users",
                newName: "IX_Users_NameEn");

            migrationBuilder.UpdateData(
                schema: "UserManagement",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "NameAr", "Password" },
                values: new object[] { "admin", "ANRY6T1hsNiVIMyBzkFP7iBPFPynnAZLDXtXJcHXR9Xj6qwVS7+WJwrvEIvH1vTCtA==" });
        }
    }
}
