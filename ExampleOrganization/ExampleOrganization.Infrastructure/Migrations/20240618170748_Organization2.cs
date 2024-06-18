using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleOrganization.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Organization2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Organizations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrganizationId",
                table: "Organizations",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Organizations_OrganizationId",
                table: "Organizations",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Organizations_OrganizationId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrganizationId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Organizations");
        }
    }
}
