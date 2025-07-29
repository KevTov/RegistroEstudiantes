using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using RegistroEstudiantes.Domain.Interfaces;
using RegistroEstudiantes.API.DTOs;

namespace RegistroEstudiantes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfesoresController : ControllerBase
{
    private readonly IProfesorRepository _profesorRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProfesoresController> _logger;

    public ProfesoresController(
        IProfesorRepository profesorRepository,
        IMapper mapper,
        ILogger<ProfesoresController> logger)
    {
        _profesorRepository = profesorRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los profesores
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProfesorDto>>>> GetAllProfesores()
    {
        try
        {
            var profesores = await _profesorRepository.GetAllAsync();
            var profesoresDto = _mapper.Map<IEnumerable<ProfesorDto>>(profesores);

            return Ok(ApiResponseDto<IEnumerable<ProfesorDto>>.SuccessResponse(
                profesoresDto,
                "Profesores obtenidos exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener profesores");
            return StatusCode(500, ApiResponseDto<IEnumerable<ProfesorDto>>.ErrorResponse(
                "Error interno del servidor"));
        }
    }

    /// <summary>
    /// Obtiene un profesor por ID con sus materias y estudiantes
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponseDto<ProfesorDto>>> GetProfesorById(int id)
    {
        try
        {
            var profesor = await _profesorRepository.GetByIdAsync(id);
            if (profesor == null)
            {
                return NotFound(ApiResponseDto<ProfesorDto>.ErrorResponse(
                    "Profesor no encontrado"));
            }

            var profesorDto = _mapper.Map<ProfesorDto>(profesor);
            return Ok(ApiResponseDto<ProfesorDto>.SuccessResponse(
                profesorDto,
                "Profesor obtenido exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener profesor {Id}", id);
            return StatusCode(500, ApiResponseDto<ProfesorDto>.ErrorResponse(
                "Error interno del servidor"));
        }
    }
}