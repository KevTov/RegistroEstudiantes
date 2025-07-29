using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Domain.Entities;
using RegistroEstudiantes.Domain.Interfaces;
using RegistroEstudiantes.Infrastructure.Data;

namespace RegistroEstudiantes.Infrastructure.Repositories;

public class MateriaRepository : IMateriaRepository
{
    private readonly ApplicationDbContext _context;

    public MateriaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Materia>> GetAllAsync()
    {
        return await _context.Materias
            .Include(m => m.Profesor)
            .Include(m => m.Inscripciones)
                .ThenInclude(i => i.Estudiante)
            .ToListAsync();
    }

    public async Task<Materia?> GetByIdAsync(int id)
    {
        return await _context.Materias
            .Include(m => m.Profesor)
            .Include(m => m.Inscripciones)
                .ThenInclude(i => i.Estudiante)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Materia>> GetMateriasDisponiblesParaEstudianteAsync(int estudianteId)
    {
        // Obtener materias en las que el estudiante NO está inscrito
        var materiasInscritas = await _context.Inscripciones
            .Where(i => i.EstudianteId == estudianteId && i.Activa)
            .Select(i => i.MateriaId)
            .ToListAsync();

        // Obtener profesores que ya tiene el estudiante
        var profesoresDelEstudiante = await _context.Inscripciones
            .Where(i => i.EstudianteId == estudianteId && i.Activa)
            .Include(i => i.Materia)
            .Select(i => i.Materia.ProfesorId)
            .ToListAsync();

        return await _context.Materias
            .Include(m => m.Profesor)
            .Where(m => !materiasInscritas.Contains(m.Id) &&
                       !profesoresDelEstudiante.Contains(m.ProfesorId))
            .ToListAsync();
    }

    public async Task<IEnumerable<Materia>> GetMateriasPorProfesorAsync(int profesorId)
    {
        return await _context.Materias
            .Include(m => m.Profesor)
            .Where(m => m.ProfesorId == profesorId)
            .ToListAsync();
    }
}