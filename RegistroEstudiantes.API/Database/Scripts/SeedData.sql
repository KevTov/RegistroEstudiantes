USE RegistroEstudiantesDB;
GO

PRINT '🌱 Iniciando inserción de datos iniciales...';

-- Descomentar estas líneas si se quiere limpiar datos existentes
/*
DELETE FROM Inscripciones;
DELETE FROM Estudiantes;
DELETE FROM Materias;
DELETE FROM Profesores;
DBCC CHECKIDENT ('Inscripciones', RESEED, 0);
DBCC CHECKIDENT ('Estudiantes', RESEED, 0);
DBCC CHECKIDENT ('Materias', RESEED, 0);
DBCC CHECKIDENT ('Profesores', RESEED, 0);
PRINT '🧹 Datos anteriores limpiados';
*/

PRINT '👨‍🏫 Insertando profesores...';

-- Verificar si ya existen profesores
IF NOT EXISTS (SELECT 1 FROM Profesores)
BEGIN
    INSERT INTO Profesores (Nombre, Apellido, Email, Especialidad) VALUES
    ('Carlos', 'Rodriguez', 'carlos.rodriguez@universidad.edu', 'Matemáticas'),
    ('María', 'González', 'maria.gonzalez@universidad.edu', 'Ciencias'),
    ('Juan', 'Pérez', 'juan.perez@universidad.edu', 'Ingeniería'),
    ('Ana', 'López', 'ana.lopez@universidad.edu', 'Humanidades'),
    ('Luis', 'Martínez', 'luis.martinez@universidad.edu', 'Tecnología');
    
    PRINT '✅ 5 profesores insertados exitosamente';
END
ELSE
BEGIN
    PRINT '⚠️ Los profesores ya existen, saltando inserción';
END

PRINT '📚 Insertando materias...';

-- Verificar si ya existen materias
IF NOT EXISTS (SELECT 1 FROM Materias)
BEGIN
    INSERT INTO Materias (Nombre, Descripcion, Creditos, ProfesorId) VALUES
    
    -- Profesor 1 - Carlos Rodriguez (Matemáticas)
    ('Cálculo I', 'Introducción al cálculo diferencial e integral', 3, 1),
    ('Álgebra Lineal', 'Estudio de vectores, matrices y transformaciones lineales', 3, 1),
    
    -- Profesor 2 - María González (Ciencias)
    ('Física I', 'Mecánica clásica y termodinámica', 3, 2),
    ('Química General', 'Fundamentos de química inorgánica y orgánica', 3, 2),
    
    -- Profesor 3 - Juan Pérez (Ingeniería)
    ('Programación I', 'Fundamentos de programación en C#', 3, 3),
    ('Estructuras de Datos', 'Algoritmos y estructuras de datos fundamentales', 3, 3),
    
    -- Profesor 4 - Ana López (Humanidades)
    ('Filosofía', 'Introducción al pensamiento filosófico', 3, 4),
    ('Ética Profesional', 'Principios éticos en el ejercicio profesional', 3, 4),
    
    -- Profesor 5 - Luis Martínez (Tecnología)
    ('Base de Datos', 'Diseño e implementación de bases de datos relacionales', 3, 5),
    ('Redes de Computadoras', 'Fundamentos de redes y protocolos de comunicación', 3, 5);
    
    PRINT '✅ 10 materias insertadas exitosamente';
END
ELSE
BEGIN
    PRINT '⚠️ Las materias ya existen, saltando inserción';
END

PRINT '🎓 Insertando estudiantes de ejemplo...';

-- Insertar algunos estudiantes de ejemplo para testing
IF NOT EXISTS (SELECT 1 FROM Estudiantes WHERE Email = 'kevin.tovar@estudiante.com')
BEGIN
    INSERT INTO Estudiantes (Nombre, Apellido, Email, Telefono, FechaRegistro) VALUES
    ('Kevin', 'Tovar', 'kevin.tovar@estudiante.com', '300-123-4567', GETDATE()),
    ('Ana', 'García', 'ana.garcia@estudiante.com', '300-234-5678', GETDATE()),
    ('Carlos', 'Mendoza', 'carlos.mendoza@estudiante.com', '300-345-6789', GETDATE());
    
    PRINT '✅ 3 estudiantes de ejemplo insertados';
END

-- Inscribir estudiantes de ejemplo a materias (respetando reglas de negocio)
IF NOT EXISTS (SELECT 1 FROM Inscripciones)
BEGIN
    -- Kevin Tovar: 3 materias de diferentes profesores
    INSERT INTO Inscripciones (EstudianteId, MateriaId, FechaInscripcion, Activa) VALUES
    (1, 1, GETDATE(), 1), -- Cálculo I (Profesor Carlos)
    (1, 3, GETDATE(), 1), -- Física I (Profesor María)  
    (1, 5, GETDATE(), 1), -- Programación I (Profesor Juan)
    
    -- Ana García: 2 materias
    (2, 2, GETDATE(), 1), -- Álgebra Lineal (Profesor Carlos)
    (2, 7, GETDATE(), 1), -- Filosofía (Profesor Ana)
    
    -- Carlos Mendoza: 3 materias
    (3, 4, GETDATE(), 1), -- Química General (Profesor María)
    (3, 6, GETDATE(), 1), -- Estructuras de Datos (Profesor Juan)
    (3, 9, GETDATE(), 1); -- Base de Datos (Profesor Luis)
    
    PRINT '✅ Inscripciones de ejemplo creadas';
END

PRINT '==========================================';
PRINT 'VERIFICACIÓN DE DATOS INSERTADOS';
PRINT '==========================================';

-- Contar registros por tabla
DECLARE @CantProfesores INT, @CantMaterias INT, @CantEstudiantes INT, @CantInscripciones INT;

SELECT @CantProfesores = COUNT(*) FROM Profesores;
SELECT @CantMaterias = COUNT(*) FROM Materias;
SELECT @CantEstudiantes = COUNT(*) FROM Estudiantes;
SELECT @CantInscripciones = COUNT(*) FROM Inscripciones WHERE Activa = 1;

PRINT '📊 ESTADÍSTICAS:';
PRINT '   👨‍🏫 Profesores: ' + CAST(@CantProfesores AS VARCHAR(10));
PRINT '   📚 Materias: ' + CAST(@CantMaterias AS VARCHAR(10));
PRINT '   🎓 Estudiantes: ' + CAST(@CantEstudiantes AS VARCHAR(10));
PRINT '   📝 Inscripciones activas: ' + CAST(@CantInscripciones AS VARCHAR(10));

-- Verificar que cada profesor tiene exactamente 2 materias
PRINT '';
PRINT '🔍 VERIFICACIÓN DE REGLAS DE NEGOCIO:';

SELECT 
    p.Nombre + ' ' + p.Apellido AS Profesor,
    p.Especialidad,
    COUNT(m.Id) AS CantidadMaterias
FROM Profesores p
LEFT JOIN Materias m ON p.Id = m.ProfesorId
GROUP BY p.Id, p.Nombre, p.Apellido, p.Especialidad
ORDER BY p.Id;

-- Verificar inscripciones por estudiante (máximo 3)
PRINT '';
PRINT '👥 INSCRIPCIONES POR ESTUDIANTE:';

SELECT 
    e.Nombre + ' ' + e.Apellido AS Estudiante,
    COUNT(i.Id) AS CantidadMaterias,
    CASE 
        WHEN COUNT(i.Id) <= 3 THEN '✅ Válido'
        ELSE '❌ Excede límite'
    END AS Estado
FROM Estudiantes e
LEFT JOIN Inscripciones i ON e.Id = i.EstudianteId AND i.Activa = 1
GROUP BY e.Id, e.Nombre, e.Apellido
ORDER BY e.Id;

-- Mostrar materias con sus profesores
PRINT '';
PRINT '📖 MATERIAS DISPONIBLES:';

SELECT 
    m.Nombre AS Materia,
    m.Creditos,
    p.Nombre + ' ' + p.Apellido AS Profesor,
    p.Especialidad
FROM Materias m
INNER JOIN Profesores p ON m.ProfesorId = p.Id
ORDER BY p.Id, m.Id;

PRINT '';
PRINT '🎉 ¡Datos iniciales insertados exitosamente!';
PRINT '🚀 El sistema está listo para usar';
PRINT '';
PRINT 'Próximos pasos:';
PRINT '1. Ejecutar la aplicación .NET Core 8';
PRINT '2. Abrir el frontend Angular 19';
PRINT '3. Probar el registro de nuevos estudiantes';

GO