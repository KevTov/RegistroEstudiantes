using RegistroEstudiantes.Domain.Entities;

namespace RegistroEstudiantes.Domain.Interfaces;

public interface IInscripcionRepository
{
    Task<IEnumerable<Inscripcion>> GetAllAsync();
    Task<IEnumerable<Inscripcion>> GetByEstudianteIdAsync(int estudianteId);
    Task<IEnumerable<Inscripcion>> GetByMateriaIdAsync(int materiaId);
    Task<Inscripcion> CreateAsync(Inscripcion inscripcion);
    Task<bool> DeleteAsync(int estudianteId, int materiaId);
    Task<bool> ExisteInscripcionAsync(int estudianteId, int materiaId);
    Task<int> GetCantidadInscripcionesPorEstudianteAsync(int estudianteId);
    Task<bool> EstudianteTieneProfesorAsync(int estudianteId, int profesorId);
}