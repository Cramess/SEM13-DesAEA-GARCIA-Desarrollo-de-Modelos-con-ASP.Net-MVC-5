using AutoMapper;
using Gimnasio.Entidades;
using Gimnasio.Entidades.Dtos;
namespace Gimnasio.Negocio.Mapeos;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Plan, PlanDto>()
            .ForMember(d => d.CantidadSocios, o => o.MapFrom(s => s.Socios.Count));
        CreateMap<PlanCrearDto, Plan>();
        CreateMap<PlanActualizarDto, Plan>();
        CreateMap<Socio, SocioDto>()
            .ForMember(d => d.Plan, o => o.MapFrom(s => s.Plan.Nombre));
        CreateMap<SocioCrearDto, Socio>()
            .ForMember(d => d.FechaInscripcion, o => o.Ignore());
        CreateMap<SocioActualizarDto, Socio>();
    }
}
