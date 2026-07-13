using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using UtnNoticias.EntityFrameworkCore;

#nullable disable

namespace UtnNoticias.Migrations
{
	[DbContext(typeof(UtnNoticiasDbContext))]
	[Migration("202604280001_AddReadingLists")]
	public partial class AddReadingLists : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "AppReadingLists",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
					OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
					CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
					LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppReadingLists", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AppReadingListItems",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ReadingListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
					Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
					Author = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
					UrlToImage = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
					PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
					Content = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
					AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppReadingListItems", x => x.Id);
					table.ForeignKey(
						name: "FK_AppReadingListItems_AppReadingLists_ReadingListId",
						column: x => x.ReadingListId,
						principalTable: "AppReadingLists",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_AppReadingListItems_ReadingListId_Url",
				table: "AppReadingListItems",
				columns: new[] { "ReadingListId", "Url" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_AppReadingLists_OwnerId_Name",
				table: "AppReadingLists",
				columns: new[] { "OwnerId", "Name" });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "AppReadingListItems");

			migrationBuilder.DropTable(
				name: "AppReadingLists");
		}
	}
}
