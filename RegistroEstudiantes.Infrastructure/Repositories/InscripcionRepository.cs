using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Domain.Entities;
using RegistroEstudiantes.Domain.Interfaces;
using RegistroEstudiantes.Infrastructure.Data;

namespace RegistroEstudiantes.Infrastructure.Repositories;

public class InscripcionRepository : IInscripcionRepository
{
    private readonly ApplicationDbContext _context;

    public InscripcionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Inscripcion>> GetAllAsync()
    {
        return await _context.Inscripciones
            .Include(i => i.Estudiante)
            .Include(i => i.Materia)
                .ThenInclude(m => m.Profesor)
            .Where(i => i.Activa)
            .ToListAsync();
    }

    public async Task<IEnumerable<Inscripcion>> GetByEstudianteIdAsync(int estudianteId)
    {
        return await _context.Inscripciones
            .Include(i => i.Materia)
                .ThenInclude(m => m.Profesor)
            .Where(i => i.EstudianteId == estudianteId && i.Activa)
            .ToListAsync();
    }

    public async Task<IEnumerable<Inscripcion>> GetByMateriaIdAsync(int materiaId)
    {
        return await _context.Inscripciones
            .Include(i => i.Estudiante)
            .Where(i => i.MateriaId == materiaId && i.Activa)
            .ToListAsync();
    }

    public async Task<Inscripcion> CreateAsync(Inscripcion inscripcion)
    {
        _context.Inscripciones.Add(inscripcion);
        await _context.SaveChangesAsync();
        return inscripcion;
    }

    public async Task<bool> DeleteAsync(int estudianteId, int materiaId)
    {
        var inscripcion = await _context.Inscripciones
            .FirstOrDefaultAsync(i => i.EstudianteId == estudianteId && i.MateriaId == materiaId);

        if (inscripcion == null)
            return false;

        inscripcion.Activa = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExisteInscripcionAsync(int estudianteId, int materiaId)
    {
        return await _context.Inscripciones
            .AnyAsync(i => i.EstudianteId == estudianteId && i.MateriaId == materiaId && i.Activa);
    }

    public async Task<int> GetCantidadInscripcionesPorEstudianteAsync(int estudianteId)
    {
        return await _context.Inscripciones
            .CountAsync(i => i.EstudianteId == estudianteId && i.Activa);
    }

    public async Task<bool> EstudianteTieneProfesorAsync(int estudianteId, int profesorId)
    {
        return await _context.Inscripciones
            .AnyAsync(i => i.EstudianteId == estudianteId &&
                          i.Materia.ProfesorId == profesorId &&
                          i.Activa);
    }
}