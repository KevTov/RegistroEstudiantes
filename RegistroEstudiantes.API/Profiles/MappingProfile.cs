using AutoMapper;
using RegistroEstudiantes.API.DTOs;
using RegistroEstudiantes.Domain.Entities;

namespace RegistroEstudiantes.API.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeos de Estudiante
        CreateMap<Estudiante, EstudianteDto>()
            .ForMember(dest => dest.Inscripciones, opt => opt.MapFrom(src => src.Inscripciones.Where(i => i.Activa)));

        CreateMap<Estudiante, EstudianteSimpleDto>();

        CreateMap<EstudianteCreateDto, Estudiante>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FechaRegistro, opt => opt.Ignore())
            .ForMember(dest => dest.Inscripciones, opt => opt.Ignore());

        CreateMap<EstudianteUpdateDto, Estudiante>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.FechaRegistro, opt => opt.Ignore())
            .ForMember(dest => dest.Inscripciones, opt => opt.Ignore());

        // Mapeos de Profesor
        CreateMap<Profesor, ProfesorDto>();
        CreateMap<Profesor, ProfesorSimpleDto>();

        // Mapeos de Materia
        CreateMap<Materia, MateriaDto>()
            .ForMember(dest => dest.Estudiantes, opt => opt.MapFrom(src =>
                src.Inscripciones.Where(i => i.Activa).Select(i => i.Estudiante)));

        CreateMap<Materia, MateriaSimpleDto>();

        // Mapeos de Inscripcion
        CreateMap<Inscripcion, InscripcionDto>()
            .ForMember(dest => dest.Profesor, opt => opt.MapFrom(src => src.Materia.Profesor));
    }
}