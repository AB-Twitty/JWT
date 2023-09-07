using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JWT.Auth.Migrations
{
    public partial class RefreshTokenTableCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2fa5a5bd-21f1-46dd-a446-24a484092886");

            migrationBuilder.CreateTable(
                name: "UserRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateExpired = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsInvalidated = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "07787195-9bde-4fd2-a347-9edb30c18558", 0, "7bd1e40b-b00e-4eab-9c81-abadeb140c64", "admin@localhost.com", false, false, null, null, null, "AQAAAAEAACcQAAAAECf1G5FbLcSSntz+EpDcXYyHvlUUpshXFQy2fDyyFjzFPV617K0mMvexShgbjG8Lkw==", null, false, "68d6979d-53c5-4b17-863a-b4b610a5a6bf", false, "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshTokens_UserId",
                table: "UserRefreshTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRefreshTokens");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "07787195-9bde-4fd2-a347-9edb30c18558");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2fa5a5bd-21f1-46dd-a446-24a484092886", 0, "31c60bbb-8527-4484-99d9-2a7c0e6aac6e", "admin@localhost.com", false, false, null, null, null, "AQAAAAEAACcQAAAAEDj9tqNBsInG2haLg9fKYWkJIujtU/BliJrMNDkS2ZOBHAiypqlQ5GehT9YlK5EwIQ==", null, false, "cd22cc03-eeb4-45b0-b7e2-ad5c4e351474", false, "Admin" });
        }
    }
}
