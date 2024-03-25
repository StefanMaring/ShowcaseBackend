using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rest_API.Migrations
{
    /// <inheritdoc />
    public partial class addedCustomUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecret",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorSecret",
                table: "AspNetUsers");
        }
    }
}
