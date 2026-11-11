using Gimnasio.Entidades.Dtos;
using Gimnasio.Negocio.Excepciones;
using Gimnasio.Negocio.Servicios;
using Microsoft.AspNetCore.Mvc;
namespace Gimnasio.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PlanesController : ControllerBase
{
    private readonly IPlanServicio _servicio;
    public PlanesController(IPlanServicio servicio)
    {
        _servicio = servicio;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlanDto>>> GetPlanes(
        [FromQuery] bool? activo, [FromQuery] decimal? precioMax)
    {
        return Ok(await _servicio.ListarAsync(activo, precioMax));
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlanDto>> GetPlan(int id)
    {
        var plan = await _servicio.ObtenerAsync(id);
        return plan == null ? NotFound() : Ok(plan);
    }
    [HttpGet("{id:int}/socios")]
    public async Task<ActionResult<IEnumerable<SocioDto>>> GetSociosDelPlan(int id)
    {
        var socios = await _servicio.ListarSociosAsync(id);
        return socios == null ? NotFound() : Ok(socios);
    }
    [HttpPost]
    public async Task<ActionResult<PlanDto>> PostPlan(PlanCrearDto dto)
    {
        try
        {
            var creado = await _servicio.CrearAsync(dto);
            return CreatedAtAction(nameof(GetPlan), new { id = creado.PlanId }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutPlan(int id, PlanActualizarDto dto)
    {
        try
        {
            return await _servicio.ActualizarAsync(id, dto) ? NoContent() : NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePlan(int id)
    {
        try
        {
            return await _servicio.EliminarAsync(id) ? NoContent() : NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
}
