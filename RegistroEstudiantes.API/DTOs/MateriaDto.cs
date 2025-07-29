namespace RegistroEstudiantes.API.DTOs;

public class MateriaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int Creditos { get; set; }
    public int ProfesorId { get; set; }
    public ProfesorSimpleDto Profesor { get; set; } = new();
    public List<EstudianteSimpleDto> Estudiantes { get; set; } = [];
}
