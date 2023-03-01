using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommercialAPI.Persistance.Migrations
{
    public partial class mig_identity_user_changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameLastname",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameLastname",
                table: "AspNetUsers");
        }
    }
}
