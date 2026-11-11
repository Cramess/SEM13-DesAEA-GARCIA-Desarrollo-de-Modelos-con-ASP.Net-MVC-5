using AutoMapper;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Entidades;
using Gimnasio.Entidades.Dtos;
using Gimnasio.Negocio.Excepciones;
namespace Gimnasio.Negocio.Servicios;
public interface IPlanServicio
{
    Task<IEnumerable<PlanDto>> ListarAsync(bool? activo, decimal? precioMax);
    Task<PlanDto?> ObtenerAsync(int id);
    Task<IEnumerable<SocioDto>?> ListarSociosAsync(int id);
    Task<PlanDto> CrearAsync(PlanCrearDto dto);
    Task<bool> ActualizarAsync(int id, PlanActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
public class PlanServicio : IPlanServicio
{
    private readonly IPlanRepositorio _planes;
    private readonly ISocioRepositorio _socios;
    private readonly IMapper _mapper;
    public PlanServicio(IPlanRepositorio planes, ISocioRepositorio socios, IMapper mapper)
    {
        _planes = planes;
        _socios = socios;
        _mapper = mapper;
    }
    public async Task<IEnumerable<PlanDto>> ListarAsync(bool? activo, decimal? precioMax)
    {
        var planes = await _planes.ListarAsync(activo, precioMax);
        return _mapper.Map<IEnumerable<PlanDto>>(planes);
    }
    public async Task<PlanDto?> ObtenerAsync(int id)
    {
        var plan = await _planes.ObtenerPorIdAsync(id);
        return plan == null ? null : _mapper.Map<PlanDto>(plan);
    }
    public async Task<IEnumerable<SocioDto>?> ListarSociosAsync(int id)
    {
        if (!await _planes.ExisteAsync(id))
            return null;
        var socios = await _socios.ListarPorPlanAsync(id);
        return _mapper.Map<IEnumerable<SocioDto>>(socios);
    }
    public async Task<PlanDto> CrearAsync(PlanCrearDto dto)
    {
        if (await _planes.ExisteNombreAsync(dto.Nombre))
            throw new ReglaNegocioException($"Ya existe un plan llamado '{dto.Nombre}'.");
        var plan = _mapper.Map<Plan>(dto);
        await _planes.AgregarAsync(plan);
        return _mapper.Map<PlanDto>(plan);
    }
    public async Task<bool> ActualizarAsync(int id, PlanActualizarDto dto)
    {
        var plan = await _planes.ObtenerParaEditarAsync(id);
        if (plan == null)
            return false;
        if (await _planes.ExisteNombreAsync(dto.Nombre, id))
            throw new ReglaNegocioException($"Ya existe otro plan llamado '{dto.Nombre}'.");
        if (plan.Activo && !dto.Activo && await _planes.TieneSociosAsync(id))
            throw new ReglaNegocioException("No se puede desactivar un plan que tiene socios inscritos.");
        _mapper.Map(dto, plan);
        await _planes.ActualizarAsync(plan);
        return true;
    }
    public async Task<bool> EliminarAsync(int id)
    {
        var plan = await _planes.ObtenerParaEditarAsync(id);
        if (plan == null)
            return false;
        if (await _planes.TieneSociosAsync(id))
            throw new ReglaNegocioException("No se puede eliminar el plan porque tiene socios inscritos.");
        await _planes.EliminarAsync(plan);
        return true;
    }
}
