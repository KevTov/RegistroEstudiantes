using System.ComponentModel.DataAnnotations;

namespace RegistroEstudiantes.Domain.Entities;

public class Profesor
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

    [StringLength(50)]
    public string Especialidad { get; set; } = string.Empty;

    // Relación con materias (cada profesor dicta 2 materias)
    public virtual ICollection<Materia> Materias { get; set; } = [];
}