if NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'RegistroEstudiantesDB')
BEGIN
    CREATE DATABASE RegistroEstudiantesDB;
    PRINT '✅ Base de datos RegistroEstudiantesDB creada exitosamente';
END
ELSE
BEGIN
    PRINT '⚠️ La base de datos RegistroEstudiantesDB ya existe';
END
GO

USE RegistroEstudiantesDB;
GO

-- 1. Tabla Profesores
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Profesores' AND xtype='U')
BEGIN
    CREATE TABLE Profesores (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre VARCHAR(100) NOT NULL,
        Apellido VARCHAR(100) NOT NULL,
        Email VARCHAR(150) NOT NULL,
        Especialidad VARCHAR(50),
        CONSTRAINT IX_Profesor_Email UNIQUE (Email)
    );
    PRINT '✅ Tabla Profesores creada';
END

-- 2. Tabla Materias
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Materias' AND xtype='U')
BEGIN
    CREATE TABLE Materias (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre VARCHAR(100) NOT NULL,
        Descripcion VARCHAR(500),
        Creditos INT NOT NULL DEFAULT 3,
        ProfesorId INT NOT NULL,
        CONSTRAINT FK_Materia_Profesor FOREIGN KEY (ProfesorId) REFERENCES Profesores(Id)
    );
    PRINT '✅ Tabla Materias creada';
END

-- 3. Tabla Estudiantes
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Estudiantes' AND xtype='U')
BEGIN
    CREATE TABLE Estudiantes (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre VARCHAR(100) NOT NULL,
        Apellido VARCHAR(100) NOT NULL,
        Email VARCHAR(150) NOT NULL,
        Telefono VARCHAR(20) NOT NULL,
        FechaRegistro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        CONSTRAINT IX_Estudiante_Email UNIQUE (Email)
    );
    PRINT '✅ Tabla Estudiantes creada';
END

-- 4. Tabla Inscripciones
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Inscripciones' AND xtype='U')
BEGIN
    CREATE TABLE Inscripciones (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EstudianteId INT NOT NULL,
        MateriaId INT NOT NULL,
        FechaInscripcion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        Activa BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_Inscripcion_Estudiante FOREIGN KEY (EstudianteId) REFERENCES Estudiantes(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Inscripcion_Materia FOREIGN KEY (MateriaId) REFERENCES Materias(Id),
        CONSTRAINT IX_Inscripcion_Estudiante_Materia UNIQUE (EstudianteId, MateriaId)
    );
    PRINT '✅ Tabla Inscripciones creada';
END

-- Índices para optimizar consultas frecuentes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Materias_ProfesorId')
BEGIN
    CREATE INDEX IX_Materias_ProfesorId ON Materias(ProfesorId);
    PRINT '✅ Índice IX_Materias_ProfesorId creado';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Inscripciones_EstudianteId')
BEGIN
    CREATE INDEX IX_Inscripciones_EstudianteId ON Inscripciones(EstudianteId);
    PRINT '✅ Índice IX_Inscripciones_EstudianteId creado';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Inscripciones_MateriaId')
BEGIN
    CREATE INDEX IX_Inscripciones_MateriaId ON Inscripciones(MateriaId);
    PRINT '✅ Índice IX_Inscripciones_MateriaId creado';
END

PRINT '==========================================';
PRINT 'VERIFICACIÓN DE ESTRUCTURA DE BASE DE DATOS';
PRINT '==========================================';

-- Contar tablas creadas
DECLARE @TotalTablas INT;
SELECT @TotalTablas = COUNT(*) 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' 
AND TABLE_NAME IN ('Profesores', 'Materias', 'Estudiantes', 'Inscripciones');

PRINT '📊 Total de tablas principales: ' + CAST(@TotalTablas AS VARCHAR(10));

-- Mostrar estructura de tablas
SELECT 
    TABLE_NAME as 'Tabla',
    COLUMN_NAME as 'Columna',
    DATA_TYPE as 'Tipo',
    IS_NULLABLE as 'Permite NULL'
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME IN ('Profesores', 'Materias', 'Estudiantes', 'Inscripciones')
ORDER BY TABLE_NAME, ORDINAL_POSITION;

PRINT '✅ Script de creación ejecutado exitosamente';
PRINT '⚡ La base de datos está lista para recibir datos';
PRINT '📝 Ejecutar SeedData.sql para insertar datos iniciales';

GO