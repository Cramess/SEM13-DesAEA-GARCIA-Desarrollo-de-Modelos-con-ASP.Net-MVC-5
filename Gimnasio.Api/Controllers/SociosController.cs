using Gimnasio.Entidades.Dtos;
using Gimnasio.Negocio.Excepciones;
using Gimnasio.Negocio.Servicios;
using Microsoft.AspNetCore.Mvc;
namespace Gimnasio.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SociosController : ControllerBase
{
    private readonly ISocioServicio _servicio;
    public SociosController(ISocioServicio servicio)
    {
        _servicio = servicio;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SocioDto>>> GetSocios(
        [FromQuery] string? nombre,
        [FromQuery] int? planId,
        [FromQuery] int? edadMin,
        [FromQuery] int? edadMax)
    {
        return Ok(await _servicio.ListarAsync(nombre, planId, edadMin, edadMax));
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SocioDto>> GetSocio(int id)
    {
        var socio = await _servicio.ObtenerAsync(id);
        return socio == null ? NotFound() : Ok(socio);
    }
    [HttpPost]
    public async Task<ActionResult<SocioDto>> PostSocio(SocioCrearDto dto)
    {
        try
        {
            var creado = await _servicio.CrearAsync(dto);
            return CreatedAtAction(nameof(GetSocio), new { id = creado.SocioId }, creado);
        }
        catch (DatoInvalidoException ex)
        {
            ModelState.AddModelError(ex.Campo, ex.Message);
            return ValidationProblem(ModelState);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutSocio(int id, SocioActualizarDto dto)
    {
        try
        {
            return await _servicio.ActualizarAsync(id, dto) ? NoContent() : NotFound();
        }
        catch (DatoInvalidoException ex)
        {
            ModelState.AddModelError(ex.Campo, ex.Message);
            return ValidationProblem(ModelState);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSocio(int id)
    {
        return await _servicio.EliminarAsync(id) ? NoContent() : NotFound();
    }
}
