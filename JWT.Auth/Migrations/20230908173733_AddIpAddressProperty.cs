using Microsoft.EntityFrameworkCore.Migrations;
using System.Net;

namespace JWT.Auth.Migrations
{
    public partial class AddIpAddressProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "UserRefreshTokens",
                type: "nvarchar(max)",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "UserRefreshTokens");
                
        }
    }
}
