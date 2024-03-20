using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rest_API.Migrations
{
    /// <inheritdoc />
    public partial class RemovedDateFromComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentDate",
                table: "Comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommentDate",
                table: "Comments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
