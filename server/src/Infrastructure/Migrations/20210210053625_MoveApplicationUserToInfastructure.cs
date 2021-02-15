using Microsoft.EntityFrameworkCore.Migrations;

namespace Recipes.Infrastructure.Migrations
{
    public partial class MoveApplicationUserToInfastructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_AspNetUsers_CreatedByUserId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_AspNetUsers_LastModifiedByUserId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructions_AspNetUsers_CreatedByUserId",
                table: "Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructions_AspNetUsers_LastModifiedByUserId",
                table: "Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeImages_AspNetUsers_CreatedByUserId",
                table: "RecipeImages");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeImages_AspNetUsers_LastModifiedByUserId",
                table: "RecipeImages");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeNotes_AspNetUsers_CreatedByUserId",
                table: "RecipeNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeNotes_AspNetUsers_LastModifiedByUserId",
                table: "RecipeNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_CreatedByUserId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_LastModifiedByUserId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipesIngredients_AspNetUsers_CreatedByUserId",
                table: "RecipesIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipesIngredients_AspNetUsers_LastModifiedByUserId",
                table: "RecipesIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipesUsers_AspNetUsers_CreatedByUserId",
                table: "RecipesUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipesUsers_AspNetUsers_LastModifiedByUserId",
                table: "RecipesUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipesUsers_AspNetUsers_UserId1",
                table: "RecipesUsers");

            migrationBuilder.DropIndex(
                name: "IX_RecipesUsers_CreatedByUserId",
                table: "RecipesUsers");

            migrationBuilder.DropIndex(
                name: "IX_RecipesUsers_LastModifiedByUserId",
                table: "RecipesUsers");

            migrationBuilder.DropIndex(
                name: "IX_RecipesIngredients_CreatedByUserId",
                table: "RecipesIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipesIngredients_LastModifiedByUserId",
                table: "RecipesIngredients");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_CreatedByUserId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_LastModifiedByUserId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_RecipeNotes_CreatedByUserId",
                table: "RecipeNotes");

            migrationBuilder.DropIndex(
                name: "IX_RecipeNotes_LastModifiedByUserId",
                table: "RecipeNotes");

            migrationBuilder.DropIndex(
                name: "IX_RecipeImages_CreatedByUserId",
                table: "RecipeImages");

            migrationBuilder.DropIndex(
                name: "IX_RecipeImages_LastModifiedByUserId",
                table: "RecipeImages");

            migrationBuilder.DropIndex(
                name: "IX_Instructions_CreatedByUserId",
                table: "Instructions");

            migrationBuilder.DropIndex(
                name: "IX_Instructions_LastModifiedByUserId",
                table: "Instructions");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_CreatedByUserId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_LastModifiedByUserId",
                table: "Ingredients");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b0cf0929-d045-45c2-bfaf-b672c46df4ee");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c329a25c-8454-4f1f-a79a-52af89053906");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "RecipesUsers",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipesUsers_UserId1",
                table: "RecipesUsers",
                newName: "IX_RecipesUsers_ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "RecipesUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "RecipesUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "RecipesIngredients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "RecipesIngredients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Recipes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Recipes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "RecipeNotes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "RecipeNotes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "RecipeImages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "RecipeImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "Instructions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Instructions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ApplicationUserId",
                table: "Recipes",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ApplicationUserId1",
                table: "Recipes",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_ApplicationUserId",
                table: "Recipes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_ApplicationUserId1",
                table: "Recipes",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesUsers_AspNetUsers_ApplicationUserId",
                table: "RecipesUsers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_ApplicationUserId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_ApplicationUserId1",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipesUsers_AspNetUsers_ApplicationUserId",
                table: "RecipesUsers");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_ApplicationUserId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_ApplicationUserId1",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "RecipesUsers",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_RecipesUsers_ApplicationUserId",
                table: "RecipesUsers",
                newName: "IX_RecipesUsers_UserId1");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "RecipesUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "RecipesUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "RecipesIngredients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "RecipesIngredients",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "Recipes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Recipes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "RecipeNotes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "RecipeNotes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "RecipeImages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "RecipeImages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "Instructions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Instructions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserId",
                table: "Ingredients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "Ingredients",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c329a25c-8454-4f1f-a79a-52af89053906", "c8e15c92-ee1a-4424-adba-68a30362e170", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b0cf0929-d045-45c2-bfaf-b672c46df4ee", "f80057c6-7c98-4708-9760-d8f42ddbc0a6", "Member", "MEMBER" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipesUsers_CreatedByUserId",
                table: "RecipesUsers",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesUsers_LastModifiedByUserId",
                table: "RecipesUsers",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_CreatedByUserId",
                table: "RecipesIngredients",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesIngredients_LastModifiedByUserId",
                table: "RecipesIngredients",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CreatedByUserId",
                table: "Recipes",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_LastModifiedByUserId",
                table: "Recipes",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeNotes_CreatedByUserId",
                table: "RecipeNotes",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeNotes_LastModifiedByUserId",
                table: "RecipeNotes",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeImages_CreatedByUserId",
                table: "RecipeImages",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeImages_LastModifiedByUserId",
                table: "RecipeImages",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_CreatedByUserId",
                table: "Instructions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_LastModifiedByUserId",
                table: "Instructions",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_CreatedByUserId",
                table: "Ingredients",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_LastModifiedByUserId",
                table: "Ingredients",
                column: "LastModifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_AspNetUsers_CreatedByUserId",
                table: "Ingredients",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_AspNetUsers_LastModifiedByUserId",
                table: "Ingredients",
                column: "LastModifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructions_AspNetUsers_CreatedByUserId",
                table: "Instructions",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructions_AspNetUsers_LastModifiedByUserId",
                table: "Instructions",
                column: "LastModifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeImages_AspNetUsers_CreatedByUserId",
                table: "RecipeImages",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeImages_AspNetUsers_LastModifiedByUserId",
                table: "RecipeImages",
                column: "LastModifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeNotes_AspNetUsers_CreatedByUserId",
                table: "RecipeNotes",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeNotes_AspNetUsers_LastModifiedByUserId",
                table: "RecipeNotes",
                column: "LastModifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_CreatedByUserId",
                table: "Recipes",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_LastModifiedByUserId",
                table: "Recipes",
                column: "LastModifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesIngredients_AspNetUsers_CreatedByUserId",
                table: "RecipesIngredients",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesIngredients_AspNetUsers_LastModifiedByUserId",
                table: "RecipesIngredients",
                column: "LastModifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesUsers_AspNetUsers_CreatedByUserId",
                table: "RecipesUsers",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesUsers_AspNetUsers_LastModifiedByUserId",
                table: "RecipesUsers",
                column: "LastModifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesUsers_AspNetUsers_UserId1",
                table: "RecipesUsers",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
