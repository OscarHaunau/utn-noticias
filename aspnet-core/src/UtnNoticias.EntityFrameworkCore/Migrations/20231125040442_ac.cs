using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtnNoticias.Migrations
{
    /// <inheritdoc />
    public partial class ac : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Themes_AbpUsers_UserId",
                table: "Themes");

            migrationBuilder.DropForeignKey(
                name: "FK_Themes_Themes_ThemeId",
                table: "Themes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Themes",
                table: "Themes");

            migrationBuilder.RenameTable(
                name: "Themes",
                newName: "AppThemes");

            migrationBuilder.RenameIndex(
                name: "IX_Themes_UserId",
                table: "AppThemes",
                newName: "IX_AppThemes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Themes_ThemeId",
                table: "AppThemes",
                newName: "IX_AppThemes_ThemeId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppThemes",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppThemes",
                table: "AppThemes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppThemes_AbpUsers_UserId",
                table: "AppThemes",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppThemes_AppThemes_ThemeId",
                table: "AppThemes",
                column: "ThemeId",
                principalTable: "AppThemes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppThemes_AbpUsers_UserId",
                table: "AppThemes");

            migrationBuilder.DropForeignKey(
                name: "FK_AppThemes_AppThemes_ThemeId",
                table: "AppThemes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppThemes",
                table: "AppThemes");

            migrationBuilder.RenameTable(
                name: "AppThemes",
                newName: "Themes");

            migrationBuilder.RenameIndex(
                name: "IX_AppThemes_UserId",
                table: "Themes",
                newName: "IX_Themes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppThemes_ThemeId",
                table: "Themes",
                newName: "IX_Themes_ThemeId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Themes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Themes",
                table: "Themes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Themes_AbpUsers_UserId",
                table: "Themes",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Themes_Themes_ThemeId",
                table: "Themes",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "Id");
        }
    }
}
