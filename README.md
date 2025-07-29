Sistema de Registro de Estudiantes - Backend
API REST desarrollada con .NET Core 8 para gestión de estudiantes y materias.
Instalación y Ejecución
Prerrequisitos

.NET 8 SDK
SQL Server / LocalDB

Ejecutar la aplicación
bashgit clone [url-repositorio]
cd RegistroEstudiantes/RegistroEstudiantes.API
dotnet run
URL de acceso: http://localhost:5186
Arquitectura del Proyecto

RegistroEstudiantes.API: Controllers y DTOs
RegistroEstudiantes.Domain: Entidades e interfaces
RegistroEstudiantes.Infrastructure: Repositorios y acceso a datos

Base de Datos

La base de datos se crea automáticamente al ejecutar la aplicación
Incluye datos iniciales: 5 profesores y 10 materias
Motor: SQL Server LocalDB

Reglas de Negocio

Máximo 3 materias por estudiante
No se puede repetir profesores en las materias seleccionadas
Cada materia equivale a 3 créditos
Email único por estudiante

API Endpoints
GET    /api/estudiantes              # Obtener todos los estudiantes
POST   /api/estudiantes              # Registrar nuevo estudiante  
GET    /api/materias                 # Obtener todas las materias
GET    /api/profesores               # Obtener todos los profesores
Tecnologías Utilizadas

.NET Core 8
Entity Framework Core
SQL Server
AutoMapper
Clean Architecture


Desarrollado por: Kevin H. Tovar L.
