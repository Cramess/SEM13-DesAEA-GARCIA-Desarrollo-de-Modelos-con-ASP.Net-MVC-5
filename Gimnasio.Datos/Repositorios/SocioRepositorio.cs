using Gimnasio.Entidades;
using Microsoft.EntityFrameworkCore;
namespace Gimnasio.Datos.Repositorios;
public interface ISocioRepositorio
{
    Task<List<Socio>> ListarAsync(string? nombre, int? planId, int? edadMin, int? edadMax);
    Task<List<Socio>> ListarPorPlanAsync(int planId);
    Task<Socio?> ObtenerPorIdAsync(int id);
    Task<Socio?> ObtenerParaEditarAsync(int id);
    Task<bool> ExisteDniAsync(string dni, int? excluirId = null);
    Task AgregarAsync(Socio socio);
    Task ActualizarAsync(Socio socio);
    Task EliminarAsync(Socio socio);
}
public class SocioRepositorio : ISocioRepositorio
{
    private readonly GimnasioContext _context;
    public SocioRepositorio(GimnasioContext context)
    {
        _context = context;
    }
    public async Task<List<Socio>> ListarAsync(string? nombre, int? planId, int? edadMin, int? edadMax)
    {
        var query = _context.Socios
            .AsNoTracking()
            .Include(s => s.Plan)
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(s => s.Nombres.Contains(nombre));
        if (planId.HasValue)
            query = query.Where(s => s.PlanId == planId.Value);
        if (edadMin.HasValue)
            query = query.Where(s => s.Edad >= edadMin.Value);
        if (edadMax.HasValue)
            query = query.Where(s => s.Edad <= edadMax.Value);
        return await query.OrderBy(s => s.Nombres).ToListAsync();
    }
    public async Task<List<Socio>> ListarPorPlanAsync(int planId) =>
        await _context.Socios
            .AsNoTracking()
            .Include(s => s.Plan)
            .Where(s => s.PlanId == planId)
            .OrderBy(s => s.Nombres)
            .ToListAsync();
    public async Task<Socio?> ObtenerPorIdAsync(int id) =>
        await _context.Socios
            .AsNoTracking()
            .Include(s => s.Plan)
            .FirstOrDefaultAsync(s => s.SocioId == id);
    public async Task<Socio?> ObtenerParaEditarAsync(int id) =>
        await _context.Socios.FindAsync(id);
    public async Task<bool> ExisteDniAsync(string dni, int? excluirId = null) =>
        await _context.Socios.AnyAsync(s => s.Dni == dni && s.SocioId != excluirId);
    public async Task AgregarAsync(Socio socio)
    {
        _context.Socios.Add(socio);
        await _context.SaveChangesAsync();
    }
    public async Task ActualizarAsync(Socio socio)
    {
        _context.Socios.Update(socio);
        await _context.SaveChangesAsync();
    }
    public async Task EliminarAsync(Socio socio)
    {
        _context.Socios.Remove(socio);
        await _context.SaveChangesAsync();
    }
}
