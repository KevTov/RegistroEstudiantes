using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Domain.Entities;
using RegistroEstudiantes.Domain.Interfaces;
using RegistroEstudiantes.Infrastructure.Data;

namespace RegistroEstudiantes.Infrastructure.Repositories;

public class EstudianteRepository : IEstudianteRepository
{
	private readonly ApplicationDbContext _context;

	public EstudianteRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Estudiante>> GetAllAsync()
	{
		return await _context.Estudiantes
			.Include(e => e.Inscripciones)
				.ThenInclude(i => i.Materia)
					.ThenInclude(m => m.Profesor)
			.ToListAsync();
	}

	public async Task<Estudiante?> GetByIdAsync(int id)
	{
		return await _context.Estudiantes
			.Include(e => e.Inscripciones)
				.ThenInclude(i => i.Materia)
					.ThenInclude(m => m.Profesor)
			.FirstOrDefaultAsync(e => e.Id == id);
	}

	public async Task<Estudiante?> GetByEmailAsync(string email)
	{
		return await _context.Estudiantes
			.Include(e => e.Inscripciones)
				.ThenInclude(i => i.Materia)
					.ThenInclude(m => m.Profesor)
			.FirstOrDefaultAsync(e => e.Email == email);
	}

	public async Task<Estudiante> CreateAsync(Estudiante estudiante)
	{
		_context.Estudiantes.Add(estudiante);
		await _context.SaveChangesAsync();
		return estudiante;
	}

	public async Task<Estudiante> UpdateAsync(Estudiante estudiante)
	{
		_context.Entry(estudiante).State = EntityState.Modified;
		await _context.SaveChangesAsync();
		return estudiante;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var estudiante = await _context.Estudiantes.FindAsync(id);
		if (estudiante == null)
			return false;

		_context.Estudiantes.Remove(estudiante);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> ExistsAsync(int id)
	{
		return await _context.Estudiantes.AnyAsync(e => e.Id == id);
	}

	public async Task<IEnumerable<Estudiante>> GetEstudiantesPorMateriaAsync(int materiaId)
	{
		return await _context.Estudiantes
			.Where(e => e.Inscripciones.Any(i => i.MateriaId == materiaId && i.Activa))
			.Include(e => e.Inscripciones)
				.ThenInclude(i => i.Materia)
			.ToListAsync();
	}
}