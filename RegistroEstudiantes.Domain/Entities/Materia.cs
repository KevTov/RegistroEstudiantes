using System.ComponentModel.DataAnnotations;

namespace RegistroEstudiantes.Domain.Entities;

public class Materia
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    public string Descripcion { get; set; } = string.Empty;

    // Cada materia equivale a 3 cr�ditos
    public int Creditos { get; set; } = 3;

    // Relaci�n con profesor (cada materia tiene un profesor)
    public int ProfesorId { get; set; }
    public virtual Profesor Profesor { get; set; } = null!;

    // Relaci�n con inscripciones
    public virtual ICollection<Inscripcion> Inscripciones { get; set; } = [];
}
