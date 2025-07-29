namespace RegistroEstudiantes.API.DTOs;

public class InscripcionDto
{
    public int Id { get; set; }
    public int EstudianteId { get; set; }
    public int MateriaId { get; set; }
    public MateriaSimpleDto Materia { get; set; } = new();
    public ProfesorSimpleDto Profesor { get; set; } = new();
    public DateTime FechaInscripcion { get; set; }
    public bool Activa { get; set; }
}
