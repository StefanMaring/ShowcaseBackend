using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rest_API.Migrations
{
    /// <inheritdoc />
    public partial class AlterCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_BlogPostId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "BlogPostId",
                table: "Comments",
                newName: "BlogPostID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_BlogPostId",
                table: "Comments",
                newName: "IX_Comments_BlogPostID");

            migrationBuilder.AlterColumn<string>(
                name: "BlogPostID",
                table: "Comments",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_BlogPostID",
                table: "Comments",
                column: "BlogPostID",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_BlogPostID",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "BlogPostID",
                table: "Comments",
                newName: "BlogPostId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_BlogPostID",
                table: "Comments",
                newName: "IX_Comments_BlogPostId");

            migrationBuilder.AlterColumn<string>(
                name: "BlogPostId",
                table: "Comments",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_BlogPostId",
                table: "Comments",
                column: "BlogPostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
