using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rest_API.Migrations
{
    /// <inheritdoc />
    public partial class AddedCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<string>(type: "TEXT", nullable: false),
                    CommentUser = table.Column<string>(type: "TEXT", nullable: false),
                    CommentDate = table.Column<string>(type: "TEXT", nullable: false),
                    CommentText = table.Column<string>(type: "TEXT", nullable: false),
                    BlogPostId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogPostId",
                table: "Comments",
                column: "BlogPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");
        }
    }
}
