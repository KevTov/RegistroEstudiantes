using RegistroEstudiantes.Domain.Entities;

namespace RegistroEstudiantes.Domain.Interfaces;

public interface IProfesorRepository
{
    Task<IEnumerable<Profesor>> GetAllAsync();
    Task<Profesor?> GetByIdAsync(int id);
    Task<IEnumerable<Profesor>> GetProfesoresPorEstudianteAsync(int estudianteId);
}