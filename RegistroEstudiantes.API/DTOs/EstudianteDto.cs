namespace RegistroEstudiantes.API.DTOs;

public class EstudianteDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
    public List<InscripcionDto> Inscripciones { get; set; } = [];
    public string NombreCompleto => $"{Nombre} {Apellido}";
}