using System.ComponentModel.DataAnnotations;

namespace RegistroEstudiantes.Domain.Entities;

public class Estudiante
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    // Relación con inscripciones (máximo 3 materias)
    public virtual ICollection<Inscripcion> Inscripciones { get; set; } = [];
}