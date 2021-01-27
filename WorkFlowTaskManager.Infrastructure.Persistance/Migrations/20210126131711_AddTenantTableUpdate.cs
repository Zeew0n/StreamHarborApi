using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkFlowTaskManager.Infrastructure.Persistance.Migrations
{
    public partial class AddTenantTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "TenantInformation");

            migrationBuilder.AddColumn<string>(
                name: "OrganizationName",
                table: "TenantInformation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationName",
                table: "TenantInformation");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TenantInformation",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
