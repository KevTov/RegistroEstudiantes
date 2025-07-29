using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RegistroEstudiantes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiantes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profesores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Especialidad = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Creditos = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    ProfesorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materias_Profesores_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inscripciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstudianteId = table.Column<int>(type: "int", nullable: false),
                    MateriaId = table.Column<int>(type: "int", nullable: false),
                    FechaInscripcion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Activa = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscripciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inscripciones_Estudiantes_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "Estudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscripciones_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Profesores",
                columns: new[] { "Id", "Apellido", "Email", "Especialidad", "Nombre" },
                values: new object[,]
                {
                    { 1, "Rodriguez", "carlos.rodriguez@universidad.edu", "Matemáticas", "Carlos" },
                    { 2, "González", "maria.gonzalez@universidad.edu", "Ciencias", "María" },
                    { 3, "Pérez", "juan.perez@universidad.edu", "Ingeniería", "Juan" },
                    { 4, "López", "ana.lopez@universidad.edu", "Humanidades", "Ana" },
                    { 5, "Martínez", "luis.martinez@universidad.edu", "Tecnología", "Luis" }
                });

            migrationBuilder.InsertData(
                table: "Materias",
                columns: new[] { "Id", "Creditos", "Descripcion", "Nombre", "ProfesorId" },
                values: new object[,]
                {
                    { 1, 3, "Introducción al cálculo diferencial e integral", "Cálculo I", 1 },
                    { 2, 3, "Estudio de vectores, matrices y transformaciones lineales", "Álgebra Lineal", 1 },
                    { 3, 3, "Mecánica clásica y termodinámica", "Física I", 2 },
                    { 4, 3, "Fundamentos de química inorgánica y orgánica", "Química General", 2 },
                    { 5, 3, "Fundamentos de programación en C#", "Programación I", 3 },
                    { 6, 3, "Algoritmos y estructuras de datos fundamentales", "Estructuras de Datos", 3 },
                    { 7, 3, "Introducción al pensamiento filosófico", "Filosofía", 4 },
                    { 8, 3, "Principios éticos en el ejercicio profesional", "Ética Profesional", 4 },
                    { 9, 3, "Diseño e implementación de bases de datos relacionales", "Base de Datos", 5 },
                    { 10, 3, "Fundamentos de redes y protocolos de comunicación", "Redes de Computadoras", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estudiante_Email",
                table: "Estudiantes",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_Estudiante_Materia",
                table: "Inscripciones",
                columns: new[] { "EstudianteId", "MateriaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_MateriaId",
                table: "Inscripciones",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_ProfesorId",
                table: "Materias",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_Profesor_Email",
                table: "Profesores",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inscripciones");

            migrationBuilder.DropTable(
                name: "Estudiantes");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "Profesores");
        }
    }
}
