using System.ComponentModel.DataAnnotations;

namespace RegistroEstudiantes.API.DTOs;

public class EstudianteCreateDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es v�lido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tel�fono es requerido")]
    [StringLength(20, ErrorMessage = "El tel�fono no puede exceder 20 caracteres")]
    [Phone(ErrorMessage = "El formato del tel�fono no es v�lido")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar al menos una materia")]
    [MaxLength(3, ErrorMessage = "No puede seleccionar m�s de 3 materias")]
    [MinLength(1, ErrorMessage = "Debe seleccionar al menos una materia")]
    public List<int> MateriasSeleccionadas { get; set; } = [];
}