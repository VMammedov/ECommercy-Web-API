﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommercialAPI.Persistance.Migrations
{
    public partial class mig_Adding_storage_to_File : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Storage",
                table: "Files",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Storage",
                table: "Files");
        }
    }
}