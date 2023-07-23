using Microsoft.EntityFrameworkCore.Migrations;

namespace EGService.DataAccess.Migrations
{
    public partial class removephone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "UserManagement",
                table: "Users");

            migrationBuilder.UpdateData(
                schema: "UserManagement",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "FirstName", "Password" },
                values: new object[] { "admin", "AKuY/Pu3vFUbaWFJGzglQEJ3K9Hgzvs/aQplT5QBJq6MeKCkjV2QnP1GpKs1ocFolg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "UserManagement",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "UserManagement",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "FirstName", "Password" },
                values: new object[] { "مدير النظام", "AI2LIgMF+0Or7FUWah+Gt5G2CZVmpQhbRdUz4RWqtnXjHhNM1R+00TT0QlmG+NcVyQ==" });
        }
    }
}
