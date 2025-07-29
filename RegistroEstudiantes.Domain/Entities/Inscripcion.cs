using System.ComponentModel.DataAnnotations;

namespace RegistroEstudiantes.Domain.Entities;

public class Inscripcion
{
    [Key]
    public int Id { get; set; }

    // Relación con estudiante
    public int EstudianteId { get; set; }
    public virtual Estudiante Estudiante { get; set; } = null!;

    // Relación con materia
    public int MateriaId { get; set; }
    public virtual Materia Materia { get; set; } = null!;

    public DateTime FechaInscripcion { get; set; } = DateTime.Now;

    // Estado de la inscripción
    public bool Activa { get; set; } = true;
}