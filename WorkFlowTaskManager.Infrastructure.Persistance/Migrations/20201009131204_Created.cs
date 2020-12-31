using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkFlowTaskManager.Infrastructure.Persistance.Migrations
{
    public partial class Created : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DatabaseHost",
                table: "UserSignup",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatabaseHost",
                table: "UserSignup");
        }
    }
}
