using RegistroEstudiantes.Domain.Entities;

namespace RegistroEstudiantes.Domain.Interfaces;

public interface IMateriaRepository
{
    Task<IEnumerable<Materia>> GetAllAsync();
    Task<Materia?> GetByIdAsync(int id);
    Task<IEnumerable<Materia>> GetMateriasDisponiblesParaEstudianteAsync(int estudianteId);
    Task<IEnumerable<Materia>> GetMateriasPorProfesorAsync(int profesorId);
}