namespace RegistroEstudiantes.API.DTOs;

public class MateriaSimpleDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int Creditos { get; set; }
}
