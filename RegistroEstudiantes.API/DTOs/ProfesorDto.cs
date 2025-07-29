namespace RegistroEstudiantes.API.DTOs;

public class ProfesorDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public List<MateriaSimpleDto> Materias { get; set; } = [];
    public string NombreCompleto => $"{Nombre} {Apellido}";
}
