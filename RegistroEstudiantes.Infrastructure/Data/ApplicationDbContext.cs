using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Domain.Entities;
using RegistroEstudiantes.Infrastructure.Configurations;

namespace RegistroEstudiantes.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets para las entidades
    public DbSet<Estudiante> Estudiantes { get; set; }
    public DbSet<Profesor> Profesores { get; set; }
    public DbSet<Materia> Materias { get; set; }
    public DbSet<Inscripcion> Inscripciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configuraciones de entidades
        modelBuilder.ApplyConfiguration(new EstudianteConfiguration());
        modelBuilder.ApplyConfiguration(new ProfesorConfiguration());
        modelBuilder.ApplyConfiguration(new MateriaConfiguration());
        modelBuilder.ApplyConfiguration(new InscripcionConfiguration());

        // Configurar relaciones y restricciones adicionales
        ConfigurarRelaciones(modelBuilder);

        // Insertar datos iniciales
        SeedData(modelBuilder);
    }

    private static void ConfigurarRelaciones(ModelBuilder modelBuilder)
    {
        // Configurar relación Materia-Profesor (muchos a uno)
        modelBuilder.Entity<Materia>()
            .HasOne(m => m.Profesor)
            .WithMany(p => p.Materias)
            .HasForeignKey(m => m.ProfesorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configurar relación Inscripcion-Estudiante (muchos a uno)
        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Estudiante)
            .WithMany(e => e.Inscripciones)
            .HasForeignKey(i => i.EstudianteId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configurar relación Inscripcion-Materia (muchos a uno)
        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Materia)
            .WithMany(m => m.Inscripciones)
            .HasForeignKey(i => i.MateriaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Crear índice único para evitar inscripciones duplicadas
        modelBuilder.Entity<Inscripcion>()
            .HasIndex(i => new { i.EstudianteId, i.MateriaId })
            .IsUnique()
            .HasDatabaseName("IX_Inscripcion_Estudiante_Materia");

        // Crear índice para email único de estudiante
        modelBuilder.Entity<Estudiante>()
            .HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("IX_Estudiante_Email");

        // Crear índice para email único de profesor
        modelBuilder.Entity<Profesor>()
            .HasIndex(p => p.Email)
            .IsUnique()
            .HasDatabaseName("IX_Profesor_Email");
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Profesores (5 profesores)
        var profesores = new[]
        {
            new Profesor { Id = 1, Nombre = "Carlos", Apellido = "Rodriguez", Email = "carlos.rodriguez@universidad.edu", Especialidad = "Matemáticas" },
            new Profesor { Id = 2, Nombre = "María", Apellido = "González", Email = "maria.gonzalez@universidad.edu", Especialidad = "Ciencias" },
            new Profesor { Id = 3, Nombre = "Juan", Apellido = "Pérez", Email = "juan.perez@universidad.edu", Especialidad = "Ingeniería" },
            new Profesor { Id = 4, Nombre = "Ana", Apellido = "López", Email = "ana.lopez@universidad.edu", Especialidad = "Humanidades" },
            new Profesor { Id = 5, Nombre = "Luis", Apellido = "Martínez", Email = "luis.martinez@universidad.edu", Especialidad = "Tecnología" }
        };

        modelBuilder.Entity<Profesor>().HasData(profesores);

        // Seed Materias (10 materias, 2 por profesor)
        var materias = new[]
        {
            // Profesor 1 - Carlos Rodriguez (Matemáticas)
            new Materia { Id = 1, Nombre = "Cálculo I", Descripcion = "Introducción al cálculo diferencial e integral", Creditos = 3, ProfesorId = 1 },
            new Materia { Id = 2, Nombre = "Álgebra Lineal", Descripcion = "Estudio de vectores, matrices y transformaciones lineales", Creditos = 3, ProfesorId = 1 },
            
            // Profesor 2 - María González (Ciencias)
            new Materia { Id = 3, Nombre = "Física I", Descripcion = "Mecánica clásica y termodinámica", Creditos = 3, ProfesorId = 2 },
            new Materia { Id = 4, Nombre = "Química General", Descripcion = "Fundamentos de química inorgánica y orgánica", Creditos = 3, ProfesorId = 2 },
            
            // Profesor 3 - Juan Pérez (Ingeniería)
            new Materia { Id = 5, Nombre = "Programación I", Descripcion = "Fundamentos de programación en C#", Creditos = 3, ProfesorId = 3 },
            new Materia { Id = 6, Nombre = "Estructuras de Datos", Descripcion = "Algoritmos y estructuras de datos fundamentales", Creditos = 3, ProfesorId = 3 },
            
            // Profesor 4 - Ana López (Humanidades)
            new Materia { Id = 7, Nombre = "Filosofía", Descripcion = "Introducción al pensamiento filosófico", Creditos = 3, ProfesorId = 4 },
            new Materia { Id = 8, Nombre = "Ética Profesional", Descripcion = "Principios éticos en el ejercicio profesional", Creditos = 3, ProfesorId = 4 },
            
            // Profesor 5 - Luis Martínez (Tecnología)
            new Materia { Id = 9, Nombre = "Base de Datos", Descripcion = "Diseño e implementación de bases de datos relacionales", Creditos = 3, ProfesorId = 5 },
            new Materia { Id = 10, Nombre = "Redes de Computadoras", Descripcion = "Fundamentos de redes y protocolos de comunicación", Creditos = 3, ProfesorId = 5 }
        };

        modelBuilder.Entity<Materia>().HasData(materias);
    }
}