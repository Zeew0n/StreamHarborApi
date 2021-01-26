using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace WorkFlowTaskManager.Infrastructure.Persistance.Migrations
{
    public partial class AddTenantTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantInformationTenantId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TenantInformation",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationEmail = table.Column<string>(nullable: true),
                    SubDomain = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantInformation", x => x.TenantId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantInformationTenantId",
                table: "AspNetUsers",
                column: "TenantInformationTenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TenantInformation_TenantInformationTenantId",
                table: "AspNetUsers",
                column: "TenantInformationTenantId",
                principalTable: "TenantInformation",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TenantInformation_TenantInformationTenantId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TenantInformation");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TenantInformationTenantId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantInformationTenantId",
                table: "AspNetUsers");
        }
    }
}
