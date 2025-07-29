using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Domain.Entities;
using RegistroEstudiantes.Domain.Interfaces;
using RegistroEstudiantes.Infrastructure.Data;

namespace RegistroEstudiantes.Infrastructure.Repositories;

public class ProfesorRepository : IProfesorRepository
{
    private readonly ApplicationDbContext _context;

    public ProfesorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Profesor>> GetAllAsync()
    {
        return await _context.Profesores
            .Include(p => p.Materias)
                .ThenInclude(m => m.Inscripciones)
            .ToListAsync();
    }

    public async Task<Profesor?> GetByIdAsync(int id)
    {
        return await _context.Profesores
            .Include(p => p.Materias)
                .ThenInclude(m => m.Inscripciones)
                    .ThenInclude(i => i.Estudiante)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Profesor>> GetProfesoresPorEstudianteAsync(int estudianteId)
    {
        return await _context.Profesores
            .Where(p => p.Materias.Any(m => m.Inscripciones.Any(i => i.EstudianteId == estudianteId && i.Activa)))
            .Include(p => p.Materias)
            .ToListAsync();
    }
}