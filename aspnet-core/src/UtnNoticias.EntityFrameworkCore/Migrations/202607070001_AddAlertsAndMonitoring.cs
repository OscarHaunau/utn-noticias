using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using UtnNoticias.EntityFrameworkCore;

#nullable disable

namespace UtnNoticias.Migrations
{
	[DbContext(typeof(UtnNoticiasDbContext))]
	[Migration("202607070001_AddAlertsAndMonitoring")]
	public partial class AddAlertsAndMonitoring : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "AppApiAccessLogs",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					SearchText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
					StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					FinishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					DurationMilliseconds = table.Column<int>(type: "int", nullable: false),
					HasError = table.Column<bool>(type: "bit", nullable: false),
					ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
					ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
					CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
					LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppApiAccessLogs", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AppNewsAlerts",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					SearchText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
					IsActive = table.Column<bool>(type: "bit", nullable: false),
					LastRunTime = table.Column<DateTime>(type: "datetime2", nullable: true),
					ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
					CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
					LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppNewsAlerts", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AppNewsNotifications",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					AlertId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
					Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					IsRead = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppNewsNotifications", x => x.Id);
					table.ForeignKey(
						name: "FK_AppNewsNotifications_AppNewsAlerts_AlertId",
						column: x => x.AlertId,
						principalTable: "AppNewsAlerts",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_AppNewsAlerts_OwnerId_SearchText",
				table: "AppNewsAlerts",
				columns: new[] { "OwnerId", "SearchText" });

			migrationBuilder.CreateIndex(
				name: "IX_AppNewsNotifications_AlertId_Url",
				table: "AppNewsNotifications",
				columns: new[] { "AlertId", "Url" },
				unique: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: "AppApiAccessLogs");
			migrationBuilder.DropTable(name: "AppNewsNotifications");
			migrationBuilder.DropTable(name: "AppNewsAlerts");
		}
	}
}
