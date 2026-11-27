using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlotixPlatform.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileAuditColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "profiles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "profiles",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "profiles");
        }
    }
}
