using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using RegistroEstudiantes.Domain.Interfaces;
using RegistroEstudiantes.API.DTOs;

namespace RegistroEstudiantes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MateriasController : ControllerBase
{
    private readonly IMateriaRepository _materiaRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<MateriasController> _logger;

    public MateriasController(
        IMateriaRepository materiaRepository,
        IMapper mapper,
        ILogger<MateriasController> logger)
    {
        _materiaRepository = materiaRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las materias disponibles
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<MateriaDto>>>> GetAllMaterias()
    {
        try
        {
            var materias = await _materiaRepository.GetAllAsync();
            var materiasDto = _mapper.Map<IEnumerable<MateriaDto>>(materias);

            return Ok(ApiResponseDto<IEnumerable<MateriaDto>>.SuccessResponse(
                materiasDto,
                "Materias obtenidas exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener materias");
            return StatusCode(500, ApiResponseDto<IEnumerable<MateriaDto>>.ErrorResponse(
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Obtiene una materia por ID con la lista de estudiantes inscritos
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponseDto<MateriaDto>>> GetMateriaById(int id)
    {
        try
        {
            var materia = await _materiaRepository.GetByIdAsync(id);
            if (materia == null)
            {
                return NotFound(ApiResponseDto<MateriaDto>.ErrorResponse(
                    "Materia no encontrada"));
            }

            var materiaDto = _mapper.Map<MateriaDto>(materia);
            return Ok(ApiResponseDto<MateriaDto>.SuccessResponse(
                materiaDto,
                "Materia obtenida exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener materia {Id}", id);
            return StatusCode(500, ApiResponseDto<MateriaDto>.ErrorResponse(
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Obtiene las materias que dicta un profesor específico
    /// </summary>
    [HttpGet("profesor/{profesorId}")]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<MateriaDto>>>> GetMateriasPorProfesor(int profesorId)
    {
        try
        {
            var materias = await _materiaRepository.GetMateriasPorProfesorAsync(profesorId);
            var materiasDto = _mapper.Map<IEnumerable<MateriaDto>>(materias);

            return Ok(ApiResponseDto<IEnumerable<MateriaDto>>.SuccessResponse(
                materiasDto,
                "Materias del profesor obtenidas exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener materias del profesor {ProfesorId}", profesorId);
            return StatusCode(500, ApiResponseDto<IEnumerable<MateriaDto>>.ErrorResponse(
                "Error interno del servidor"));
        }
    }
}