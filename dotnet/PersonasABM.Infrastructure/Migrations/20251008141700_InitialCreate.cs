using System;
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
            migrationBuilder.CreateTable(
                name: "atributo_tipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    TipoDato = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    EsObligatorio = table.Column<bool>(type: "INTEGER", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_atributo_tipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "personas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreCompleto = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Identificacion = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Edad = table.Column<int>(type: "INTEGER", nullable: false),
                    Genero = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Estado = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AtributosAdicionales = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "persona_atributos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonaId = table.Column<int>(type: "INTEGER", nullable: false),
                    AtributoTipoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Valor = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persona_atributos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_persona_atributos_atributo_tipos_AtributoTipoId",
                        column: x => x.AtributoTipoId,
                        principalTable: "atributo_tipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_persona_atributos_personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "atributo_tipos",
                columns: new[] { "Id", "Activo", "Descripcion", "EsObligatorio", "FechaCreacion", "Nombre", "TipoDato" },
                values: new object[,]
                {
                    { 1, true, "¿La persona maneja vehículos?", false, new DateTime(2025, 10, 8, 14, 16, 59, 371, DateTimeKind.Utc).AddTicks(8766), "Maneja", "Booleano" },
                    { 2, true, "¿La persona usa lentes correctivos?", false, new DateTime(2025, 10, 8, 14, 16, 59, 371, DateTimeKind.Utc).AddTicks(8775), "Usa Lentes", "Booleano" },
                    { 3, true, "¿La persona tiene diabetes?", false, new DateTime(2025, 10, 8, 14, 16, 59, 371, DateTimeKind.Utc).AddTicks(8781), "Es Diabético", "Booleano" },
                    { 4, true, "Tipo de sangre de la persona", false, new DateTime(2025, 10, 8, 14, 16, 59, 371, DateTimeKind.Utc).AddTicks(8783), "Tipo de Sangre", "Texto" }
                });

            migrationBuilder.InsertData(
                table: "personas",
                columns: new[] { "Id", "AtributosAdicionales", "Edad", "Estado", "FechaCreacion", "FechaModificacion", "Genero", "Identificacion", "NombreCompleto" },
                values: new object[,]
                {
                    { 1, "{\"Maneja\": true, \"Usa Lentes\": false, \"Es Diabético\": false, \"Tipo de Sangre\": \"O+\"}", 35, "Activo", new DateTime(2025, 10, 8, 14, 16, 59, 371, DateTimeKind.Utc).AddTicks(9206), null, "Masculino", "12345678", "Juan Pérez García" },
                    { 2, "{\"Maneja\": true, \"Usa Lentes\": true, \"Es Diabético\": true, \"Tipo de Sangre\": \"A+\"}", 28, "Activo", new DateTime(2025, 10, 8, 14, 16, 59, 371, DateTimeKind.Utc).AddTicks(9209), null, "Femenino", "87654321", "María López Rodríguez" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_persona_atributos_AtributoTipoId",
                table: "persona_atributos",
                column: "AtributoTipoId");

            migrationBuilder.CreateIndex(
                name: "IX_persona_atributos_PersonaId",
                table: "persona_atributos",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_personas_Identificacion",
                table: "personas",
                column: "Identificacion",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "persona_atributos");

            migrationBuilder.DropTable(
                name: "atributo_tipos");

            migrationBuilder.DropTable(
                name: "personas");
        }
    }
}
