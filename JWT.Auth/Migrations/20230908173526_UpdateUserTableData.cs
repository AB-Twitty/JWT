using Microsoft.EntityFrameworkCore.Migrations;

namespace JWT.Auth.Migrations
{
    public partial class UpdateUserTableData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a3f1f33a-2f89-49e7-bad1-71075a13f7d1", 0, "a19d03cb-8b52-41f6-93af-1af5b76ae717", "admin@localhost.com", false, false, null, "ADMIN@LOCALHOST.COM", "ADMIN", "AQAAAAEAACcQAAAAELJ6m3UQWz8Zm54XueiZRQkY2ok30WRfIFPLtMs5CHVpZVSYaIurrFEgPUgVBYDqOA==", null, false, "61e403c4-20c7-4f35-b953-312048c80210", false, "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a3f1f33a-2f89-49e7-bad1-71075a13f7d1");
        }
    }
}
