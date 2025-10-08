using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PersonasABM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AtributoTipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoDato = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsObligatorio = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtributoTipos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreCompleto = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Identificacion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    Genero = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AtributosAdicionales = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PersonaAtributos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PersonaId = table.Column<int>(type: "int", nullable: false),
                    AtributoTipoId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonaAtributos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonaAtributos_AtributoTipos_AtributoTipoId",
                        column: x => x.AtributoTipoId,
                        principalTable: "AtributoTipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonaAtributos_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AtributoTipos",
                columns: new[] { "Id", "Activo", "Descripcion", "EsObligatorio", "FechaCreacion", "Nombre", "TipoDato" },
                values: new object[,]
                {
                    { 1, true, "¿La persona maneja vehículos?", false, new DateTime(2025, 10, 7, 17, 14, 55, 101, DateTimeKind.Utc).AddTicks(5666), "Maneja", "Booleano" },
                    { 2, true, "¿La persona usa lentes correctivos?", false, new DateTime(2025, 10, 7, 17, 14, 55, 101, DateTimeKind.Utc).AddTicks(5756), "Usa Lentes", "Booleano" },
                    { 3, true, "¿La persona tiene diabetes?", false, new DateTime(2025, 10, 7, 17, 14, 55, 101, DateTimeKind.Utc).AddTicks(5794), "Es Diabético", "Booleano" },
                    { 4, true, "Tipo de sangre de la persona", false, new DateTime(2025, 10, 7, 17, 14, 55, 101, DateTimeKind.Utc).AddTicks(5832), "Tipo de Sangre", "Texto" }
                });

            migrationBuilder.InsertData(
                table: "Personas",
                columns: new[] { "Id", "AtributosAdicionales", "Edad", "Estado", "FechaCreacion", "FechaModificacion", "Genero", "Identificacion", "NombreCompleto" },
                values: new object[,]
                {
                    { 1, "{\"Maneja\": true, \"Usa Lentes\": false, \"Es Diabético\": false, \"Tipo de Sangre\": \"O+\"}", 35, "Activo", new DateTime(2025, 10, 7, 17, 14, 55, 101, DateTimeKind.Utc).AddTicks(6103), null, "Masculino", "12345678", "Juan Pérez García" },
                    { 2, "{\"Maneja\": true, \"Usa Lentes\": true, \"Es Diabético\": true, \"Tipo de Sangre\": \"A+\"}", 28, "Activo", new DateTime(2025, 10, 7, 17, 14, 55, 101, DateTimeKind.Utc).AddTicks(6152), null, "Femenino", "87654321", "María López Rodríguez" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonaAtributos_AtributoTipoId",
                table: "PersonaAtributos",
                column: "AtributoTipoId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaAtributos_PersonaId",
                table: "PersonaAtributos",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_Identificacion",
                table: "Personas",
                column: "Identificacion",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonaAtributos");

            migrationBuilder.DropTable(
                name: "AtributoTipos");

            migrationBuilder.DropTable(
                name: "Personas");
        }
    }
}
