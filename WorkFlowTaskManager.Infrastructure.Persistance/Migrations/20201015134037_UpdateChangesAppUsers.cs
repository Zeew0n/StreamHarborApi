using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkFlowTaskManager.Infrastructure.Persistance.Migrations
{
    public partial class UpdateChangesAppUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizationName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subdomain",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Subdomain",
                table: "AspNetUsers");
        }
    }
}
