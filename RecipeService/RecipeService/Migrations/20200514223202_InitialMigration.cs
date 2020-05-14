using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeService.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Rating = table.Column<decimal>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: true),
                    Password = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IngredientId = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    RecipeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IngredientId);
                    table.ForeignKey(
                        name: "FK_Ingredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Instructions",
                columns: table => new
                {
                    InstructionId = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    RecipeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructions", x => x.InstructionId);
                    table.ForeignKey(
                        name: "FK_Instructions_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "IsDeleted", "Name", "Notes", "Rating" },
                values: new object[,]
                {
                    { new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"), false, "Pho", "For best results, use shank and knee bones.", 4.7m },
                    { new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"), false, "Polish Perogies", "For best results, choose potatoes that have as little water in them as possible such as Russets.", 4.1m },
                    { new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"), false, "Bread", "Use active yeast for best results.", 3.2m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Password", "UserName" },
                values: new object[] { new Guid("96d9a443-db55-4881-9b8c-4318eb5ae17a"), "admin", "admin" });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "IngredientId", "RecipeId", "Value" },
                values: new object[,]
                {
                    { new Guid("29346366-c8bb-42f4-a3d8-3c69d9896b73"), new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"), "Water" },
                    { new Guid("639aa8f9-f129-460e-8437-eac45558b320"), new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"), "Salt" },
                    { new Guid("f19633c8-1893-47fa-9ae2-7ca6f3176361"), new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"), "Flour" },
                    { new Guid("debee29b-de56-46a5-acf1-73d242d22727"), new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"), "Water" },
                    { new Guid("0425b446-866f-4316-84ec-f33eacec5605"), new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"), "Flour" },
                    { new Guid("659e5798-a2fa-4405-883d-8b12ffe109ef"), new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"), "Water" },
                    { new Guid("f6ef45f0-172a-4e84-8b63-ff6b704cfcdc"), new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"), "Salt" },
                    { new Guid("7d30c147-6405-4f53-b941-cca64c6afe3b"), new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"), "Salt" },
                    { new Guid("9756a3d4-87a8-4e4e-9ced-c3dd35c38067"), new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"), "Flour" }
                });

            migrationBuilder.InsertData(
                table: "Instructions",
                columns: new[] { "InstructionId", "RecipeId", "SortOrder", "Value" },
                values: new object[,]
                {
                    { new Guid("2613dc32-9b1b-4307-930f-20be88eb0b14"), new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"), 2, "Insert food." },
                    { new Guid("93d529aa-e50f-4f2d-8dfa-61e3753daf3d"), new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"), 2, "Insert food." },
                    { new Guid("34fb44b8-d23c-429e-9290-dad66ddd1f9f"), new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"), 1, "Preheat oven to 450F." },
                    { new Guid("6c43866b-1297-449d-9319-341a5146ab8f"), new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"), 2, "Insert food." },
                    { new Guid("0cbe392c-e893-46b8-89ba-ab5e9ce80977"), new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"), 3, "Take out food after 15 minutes." },
                    { new Guid("a3f12a50-0613-4f39-8dd9-c2265d994ab9"), new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"), 1, "Preheat oven to 450F." },
                    { new Guid("9da11bcf-b092-4aba-b46a-54b43e7227df"), new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"), 1, "Preheat oven to 450F." },
                    { new Guid("10db9bee-f528-44fb-9f66-0f5219194366"), new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"), 3, "Take out food after 15 minutes." },
                    { new Guid("44325639-afc7-43ad-8701-a17e88cf5b04"), new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"), 3, "Take out food after 15 minutes." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_RecipeId",
                table: "Ingredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_RecipeId",
                table: "Instructions",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Instructions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
