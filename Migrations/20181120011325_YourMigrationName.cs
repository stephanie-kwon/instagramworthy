using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InstagramWorthy.Migrations
{
    public partial class YourMigrationName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Created_at = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    ProfileUrl = table.Column<string>(nullable: true),
                    Updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "places",
                columns: table => new
                {
                    PlaceId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Description = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PicUrl = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_places", x => x.PlaceId);
                    table.ForeignKey(
                        name: "FK_places_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    LikeId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    PlaceId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_likes", x => x.LikeId);
                    table.ForeignKey(
                        name: "FK_likes_places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "places",
                        principalColumn: "PlaceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_likes_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Description = table.Column<string>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PicUrl = table.Column<string>(nullable: true),
                    reviewPlaceId = table.Column<int>(nullable: true),
                    reviewuploaderUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_reviews_places_reviewPlaceId",
                        column: x => x.reviewPlaceId,
                        principalTable: "places",
                        principalColumn: "PlaceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reviews_users_reviewuploaderUserId",
                        column: x => x.reviewuploaderUserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_likes_PlaceId",
                table: "likes",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_likes_UserId",
                table: "likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_places_UserId",
                table: "places",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_reviewPlaceId",
                table: "reviews",
                column: "reviewPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_reviewuploaderUserId",
                table: "reviews",
                column: "reviewuploaderUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "places");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
