using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using RegistroEstudiantes.Domain.Entities;
using RegistroEstudiantes.Domain.Interfaces;
using RegistroEstudiantes.API.DTOs;

namespace RegistroEstudiantes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstudiantesController : ControllerBase
{
    private readonly IEstudianteRepository _estudianteRepository;
    private readonly IMateriaRepository _materiaRepository;
    private readonly IInscripcionRepository _inscripcionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<EstudiantesController> _logger;

    public EstudiantesController(
        IEstudianteRepository estudianteRepository,
        IMateriaRepository materiaRepository,
        IInscripcionRepository inscripcionRepository,
        IMapper mapper,
        ILogger<EstudiantesController> logger)
    {
        _estudianteRepository = estudianteRepository;
        _materiaRepository = materiaRepository;
        _inscripcionRepository = inscripcionRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los estudiantes registrados
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<EstudianteDto>>>> GetAllEstudiantes()
    {
        try
        {
            var estudiantes = await _estudianteRepository.GetAllAsync();
            var estudiantesDto = _mapper.Map<IEnumerable<EstudianteDto>>(estudiantes);

            return Ok(ApiResponseDto<IEnumerable<EstudianteDto>>.SuccessResponse(
                estudiantesDto,
                "Estudiantes obtenidos exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estudiantes");
            return StatusCode(500, ApiResponseDto<IEnumerable<EstudianteDto>>.ErrorResponse(
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Obtiene un estudiante por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponseDto<EstudianteDto>>> GetEstudianteById(int id)
    {
        try
        {
            var estudiante = await _estudianteRepository.GetByIdAsync(id);
            if (estudiante == null)
            {
                return NotFound(ApiResponseDto<EstudianteDto>.ErrorResponse(
                    "Estudiante no encontrado"));
            }

            var estudianteDto = _mapper.Map<EstudianteDto>(estudiante);
            return Ok(ApiResponseDto<EstudianteDto>.SuccessResponse(
                estudianteDto,
                "Estudiante obtenido exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estudiante {Id}", id);
            return StatusCode(500, ApiResponseDto<EstudianteDto>.ErrorResponse(
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Registra un nuevo estudiante con sus materias seleccionadas
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponseDto<EstudianteDto>>> CreateEstudiante(EstudianteCreateDto estudianteCreateDto)
    {
        try
        {
            // Validar que el email no esté en uso
            var estudianteExistente = await _estudianteRepository.GetByEmailAsync(estudianteCreateDto.Email);
            if (estudianteExistente != null)
            {
                return BadRequest(ApiResponseDto<EstudianteDto>.ErrorResponse(
                    "El email ya está registrado"));
            }

            // Validar número de materias (máximo 3)
            if (estudianteCreateDto.MateriasSeleccionadas.Count > 3)
            {
                return BadRequest(ApiResponseDto<EstudianteDto>.ErrorResponse(
                    "No puede seleccionar más de 3 materias"));
            }

            if (estudianteCreateDto.MateriasSeleccionadas.Count == 0)
            {
                return BadRequest(ApiResponseDto<EstudianteDto>.ErrorResponse(
                    "Debe seleccionar al menos una materia"));
            }

            // Validar que las materias existan
            var materias = new List<Materia>();
            foreach (var materiaId in estudianteCreateDto.MateriasSeleccionadas)
            {
                var materia = await _materiaRepository.GetByIdAsync(materiaId);
                if (materia == null)
                {
                    return BadRequest(ApiResponseDto<EstudianteDto>.ErrorResponse(
                        $"La materia con ID {materiaId} no existe"));
                }
                materias.Add(materia);
            }

            // Validar que no haya materias del mismo profesor
            var profesoresIds = materias.Select(m => m.ProfesorId).ToList();
            if (profesoresIds.Distinct().Count() != profesoresIds.Count)
            {
                return BadRequest(ApiResponseDto<EstudianteDto>.ErrorResponse(
                    "No puede seleccionar materias del mismo profesor"));
            }

            // Crear el estudiante
            var estudiante = _mapper.Map<Estudiante>(estudianteCreateDto);
            var estudianteCreado = await _estudianteRepository.CreateAsync(estudiante);

            // Crear las inscripciones
            foreach (var materiaId in estudianteCreateDto.MateriasSeleccionadas)
            {
                var inscripcion = new Inscripcion
                {
                    EstudianteId = estudianteCreado.Id,
                    MateriaId = materiaId,
                    FechaInscripcion = DateTime.Now,
                    Activa = true
                };
                await _inscripcionRepository.CreateAsync(inscripcion);
            }

            // Obtener el estudiante completo con sus inscripciones
            var estudianteCompleto = await _estudianteRepository.GetByIdAsync(estudianteCreado.Id);
            var estudianteDto = _mapper.Map<EstudianteDto>(estudianteCompleto);

            return CreatedAtAction(
                nameof(GetEstudianteById),
                new { id = estudianteCreado.Id },
                ApiResponseDto<EstudianteDto>.SuccessResponse(
                    estudianteDto,
                    "Estudiante registrado exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear estudiante");
            return StatusCode(500, ApiResponseDto<EstudianteDto>.ErrorResponse(
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Actualiza la información básica de un estudiante (no las materias)
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponseDto<EstudianteDto>>> UpdateEstudiante(int id, EstudianteUpdateDto estudianteUpdateDto)
    {
        try
        {
            var estudiante = await _estudianteRepository.GetByIdAsync(id);
            if (estudiante == null)
            {
                return NotFound(ApiResponseDto<EstudianteDto>.ErrorResponse(
                    "Estudiante no encontrado"));
            }

            // Actualizar solo los campos permitidos
            _mapper.Map(estudianteUpdateDto, estudiante);

            var estudianteActualizado = await _estudianteRepository.UpdateAsync(estudiante);
            var estudianteDto = _mapper.Map<EstudianteDto>(estudianteActualizado);

            return Ok(ApiResponseDto<EstudianteDto>.SuccessResponse(
                estudianteDto,
                "Estudiante actualizado exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar estudiante {Id}", id);
            return StatusCode(500, ApiResponseDto<EstudianteDto>.ErrorResponse(
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Elimina un estudiante y todas sus inscripciones
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponseDto<object>>> DeleteEstudiante(int id)
    {
        try
        {
            var estudiante = await _estudianteRepository.GetByIdAsync(id);
            if (estudiante == null)
            {
                return NotFound(ApiResponseDto<object>.ErrorResponse(
                    "Estudiante no encontrado"));
            }

            var eliminado = await _estudianteRepository.DeleteAsync(id);
            if (!eliminado)
            {
                return StatusCode(500, ApiResponseDto<object>.ErrorResponse(
                    "Error al eliminar el estudiante"));
            }

            return Ok(ApiResponseDto<object>.SuccessResponse(
                null,
                "Estudiante eliminado exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar estudiante {Id}", id);
            return StatusCode(500, ApiResponseDto<object>.ErrorResponse(
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Obtiene las materias disponibles para un estudiante (no inscrito y sin profesor repetido)
    /// </summary>
    [HttpGet("{id}/materias-disponibles")]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<MateriaDto>>>> GetMateriasDisponibles(int id)
    {
        try
        {
            var estudiante = await _estudianteRepository.GetByIdAsync(id);
            if (estudiante == null)
            {
                return NotFound(ApiResponseDto<IEnumerable<MateriaDto>>.ErrorResponse(
                    "Estudiante no encontrado"));
            }

            // Verificar que el estudiante no tenga ya 3 materias
            var cantidadInscripciones = await _inscripcionRepository.GetCantidadInscripcionesPorEstudianteAsync(id);
            if (cantidadInscripciones >= 3)
            {
                return Ok(ApiResponseDto<IEnumerable<MateriaDto>>.SuccessResponse(
                    new List<MateriaDto>(),
                    "El estudiante ya tiene el máximo de materias permitidas"));
            }

            var materiasDisponibles = await _materiaRepository.GetMateriasDisponiblesParaEstudianteAsync(id);
            var materiasDto = _mapper.Map<IEnumerable<MateriaDto>>(materiasDisponibles);

            return Ok(ApiResponseDto<IEnumerable<MateriaDto>>.SuccessResponse(
                materiasDto,
                "Materias disponibles obtenidas exitosamente :)"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener materias disponibles para estudiante {Id}", id);
            return StatusCode(500, ApiResponseDto<IEnumerable<MateriaDto>>.ErrorResponse(
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Obtiene los compañeros de clase de un estudiante por materia
    /// </summary>

    [HttpGet("{id}/companeros")]
    public async Task<ActionResult<ApiResponseDto<Dictionary<string, List<EstudianteSimpleDto>>>>> GetCompanerosDeClase(int id)
    {
        try
        {
            var estudiante = await _estudianteRepository.GetByIdAsync(id);
            if (estudiante == null)
            {
                return NotFound(ApiResponseDto<Dictionary<string, List<EstudianteSimpleDto>>>.ErrorResponse(
                    "Estudiante no encontrado "));
            }

            var companerosPorMateria = new Dictionary<string, List<EstudianteSimpleDto>>();

            var inscripciones = await _inscripcionRepository.GetByEstudianteIdAsync(id);

            foreach (var inscripcion in inscripciones)
            {
                var companeros = await _estudianteRepository.GetEstudiantesPorMateriaAsync(inscripcion.MateriaId);
                var companerosExceptoActual = companeros.Where(e => e.Id != id);
                var companerosDto = _mapper.Map<List<EstudianteSimpleDto>>(companerosExceptoActual);

                companerosPorMateria[inscripcion.Materia.Nombre] = companerosDto;
            }

            return Ok(ApiResponseDto<Dictionary<string, List<EstudianteSimpleDto>>>.SuccessResponse(
                companerosPorMateria,
                "Compañeros de clase obtenidos exitosamente :)"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener compañeros de clase para estudiante {Id}", id);
            return StatusCode(500, ApiResponseDto<Dictionary<string, List<EstudianteSimpleDto>>>.ErrorResponse(
                "Error interno del servidor"));
        }
    }
}