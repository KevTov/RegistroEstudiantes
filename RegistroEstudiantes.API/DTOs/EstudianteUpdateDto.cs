using System.ComponentModel.DataAnnotations;

namespace RegistroEstudiantes.API.DTOs;

public class EstudianteUpdateDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
    public string Apellido { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El teléfono es requerido")]
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    [Phone(ErrorMessage = "El formato del teléfono no es válido")]
    public string Telefono { get; set; } = string.Empty;
}
