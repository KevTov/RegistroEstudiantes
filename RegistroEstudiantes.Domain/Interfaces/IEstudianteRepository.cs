using RegistroEstudiantes.Domain.Entities;

namespace RegistroEstudiantes.Domain.Interfaces;

public interface IEstudianteRepository
{
    Task<IEnumerable<Estudiante>> GetAllAsync();
    Task<Estudiante?> GetByIdAsync(int id);
    Task<Estudiante?> GetByEmailAsync(string email);
    Task<Estudiante> CreateAsync(Estudiante estudiante);
    Task<Estudiante> UpdateAsync(Estudiante estudiante);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Estudiante>> GetEstudiantesPorMateriaAsync(int materiaId);
}