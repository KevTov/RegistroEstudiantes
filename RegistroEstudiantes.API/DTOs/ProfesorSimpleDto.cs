namespace RegistroEstudiantes.API.DTOs;

public class ProfesorSimpleDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public string NombreCompleto => $"{Nombre} {Apellido}";
}
