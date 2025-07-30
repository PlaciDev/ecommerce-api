using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommerceApi.Migrations
{
    public partial class AjusteIdsRelacionamentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Customer",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Customer",
                type: "VARCHAR",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20);
        }
    }
}
