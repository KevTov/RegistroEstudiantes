using System.ComponentModel.DataAnnotations;

namespace RegistroEstudiantes.Domain.Entities;

public class Inscripcion
{
    [Key]
    public int Id { get; set; }

    // Relaci�n con estudiante
    public int EstudianteId { get; set; }
    public virtual Estudiante Estudiante { get; set; } = null!;

    // Relaci�n con materia
    public int MateriaId { get; set; }
    public virtual Materia Materia { get; set; } = null!;

    public DateTime FechaInscripcion { get; set; } = DateTime.Now;

    // Estado de la inscripci�n
    public bool Activa { get; set; } = true;
}