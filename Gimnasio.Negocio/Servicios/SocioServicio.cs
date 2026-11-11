using AutoMapper;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Entidades;
using Gimnasio.Entidades.Dtos;
using Gimnasio.Negocio.Excepciones;
namespace Gimnasio.Negocio.Servicios;
public interface ISocioServicio
{
    Task<IEnumerable<SocioDto>> ListarAsync(string? nombre, int? planId, int? edadMin, int? edadMax);
    Task<SocioDto?> ObtenerAsync(int id);
    Task<SocioDto> CrearAsync(SocioCrearDto dto);
    Task<bool> ActualizarAsync(int id, SocioActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
public class SocioServicio : ISocioServicio
{
    private readonly ISocioRepositorio _socios;
    private readonly IPlanRepositorio _planes;
    private readonly IMapper _mapper;
    public SocioServicio(ISocioRepositorio socios, IPlanRepositorio planes, IMapper mapper)
    {
        _socios = socios;
        _planes = planes;
        _mapper = mapper;
    }
    public async Task<IEnumerable<SocioDto>> ListarAsync(string? nombre, int? planId, int? edadMin, int? edadMax)
    {
        var socios = await _socios.ListarAsync(nombre, planId, edadMin, edadMax);
        return _mapper.Map<IEnumerable<SocioDto>>(socios);
    }
    public async Task<SocioDto?> ObtenerAsync(int id)
    {
        var socio = await _socios.ObtenerPorIdAsync(id);
        return socio == null ? null : _mapper.Map<SocioDto>(socio);
    }
    public async Task<SocioDto> CrearAsync(SocioCrearDto dto)
    {
        await ValidarPlanAsync(dto.PlanId);
        if (await _socios.ExisteDniAsync(dto.Dni))
            throw new ReglaNegocioException($"Ya existe un socio con el DNI {dto.Dni}.");
        var socio = _mapper.Map<Socio>(dto);
        socio.FechaInscripcion = DateOnly.FromDateTime(DateTime.Today);
        await _socios.AgregarAsync(socio);
        var creado = await _socios.ObtenerPorIdAsync(socio.SocioId);
        return _mapper.Map<SocioDto>(creado);
    }
    public async Task<bool> ActualizarAsync(int id, SocioActualizarDto dto)
    {
        var socio = await _socios.ObtenerParaEditarAsync(id);
        if (socio == null)
            return false;
        if (dto.PlanId != socio.PlanId)
            await ValidarPlanAsync(dto.PlanId);
        if (await _socios.ExisteDniAsync(dto.Dni, id))
            throw new ReglaNegocioException($"Ya existe otro socio con el DNI {dto.Dni}.");
        _mapper.Map(dto, socio);
        await _socios.ActualizarAsync(socio);
        return true;
    }
    public async Task<bool> EliminarAsync(int id)
    {
        var socio = await _socios.ObtenerParaEditarAsync(id);
        if (socio == null)
            return false;
        await _socios.EliminarAsync(socio);
        return true;
    }
    private async Task ValidarPlanAsync(int planId)
    {
        var plan = await _planes.ObtenerPorIdAsync(planId);
        if (plan == null)
            throw new DatoInvalidoException(nameof(SocioCrearDto.PlanId), $"No existe el plan con id {planId}.");
        if (!plan.Activo)
            throw new ReglaNegocioException($"El plan '{plan.Nombre}' no está activo y no admite nuevas inscripciones.");
    }
}
