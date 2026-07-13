using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtnNoticias.Migrations
{
    /// <inheritdoc />
    public partial class aa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alertas_ListaContenedores_listacontenedorId",
                table: "Alertas");

            migrationBuilder.DropForeignKey(
                name: "FK_IComponente_ListaContenedores_ListaContenedorId",
                table: "IComponente");

            migrationBuilder.DropTable(
                name: "ListaContenedores");

            migrationBuilder.DropTable(
                name: "Noticias");

            migrationBuilder.AddColumn<string>(
                name: "Autor",
                table: "IComponente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contenido",
                table: "IComponente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "IComponente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "IComponente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPublicacion",
                table: "IComponente",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "IComponente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkImagen",
                table: "IComponente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "IComponente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tema",
                table: "IComponente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "IComponente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "fechaLectura",
                table: "IComponente",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alertas_IComponente_listacontenedorId",
                table: "Alertas",
                column: "listacontenedorId",
                principalTable: "IComponente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IComponente_IComponente_ListaContenedorId",
                table: "IComponente",
                column: "ListaContenedorId",
                principalTable: "IComponente",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alertas_IComponente_listacontenedorId",
                table: "Alertas");

            migrationBuilder.DropForeignKey(
                name: "FK_IComponente_IComponente_ListaContenedorId",
                table: "IComponente");

            migrationBuilder.DropColumn(
                name: "Autor",
                table: "IComponente");

            migrationBuilder.DropColumn(
                name: "Contenido",
                table: "IComponente");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "IComponente");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "IComponente");

            migrationBuilder.DropColumn(
                name: "FechaPublicacion",
                table: "IComponente");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "IComponente");

            migrationBuilder.DropColumn(
                name: "LinkImagen",
                table: "IComponente");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "IComponente");

            migrationBuilder.DropColumn(
                name: "Tema",
                table: "IComponente");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "IComponente");

            migrationBuilder.DropColumn(
                name: "fechaLectura",
                table: "IComponente");

            migrationBuilder.CreateTable(
                name: "ListaContenedores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaContenedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Noticias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkImagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fechaLectura = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Noticias", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Alertas_ListaContenedores_listacontenedorId",
                table: "Alertas",
                column: "listacontenedorId",
                principalTable: "ListaContenedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IComponente_ListaContenedores_ListaContenedorId",
                table: "IComponente",
                column: "ListaContenedorId",
                principalTable: "ListaContenedores",
                principalColumn: "Id");
        }
    }
}
