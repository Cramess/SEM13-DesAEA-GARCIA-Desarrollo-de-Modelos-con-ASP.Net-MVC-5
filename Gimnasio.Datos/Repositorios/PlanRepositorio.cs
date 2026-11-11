using Gimnasio.Entidades;
using Microsoft.EntityFrameworkCore;
namespace Gimnasio.Datos.Repositorios;
public interface IPlanRepositorio
{
    Task<List<Plan>> ListarAsync(bool? activo, decimal? precioMax);
    Task<Plan?> ObtenerPorIdAsync(int id);
    Task<Plan?> ObtenerParaEditarAsync(int id);
    Task<bool> ExisteAsync(int id);
    Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null);
    Task<bool> TieneSociosAsync(int id);
    Task AgregarAsync(Plan plan);
    Task ActualizarAsync(Plan plan);
    Task EliminarAsync(Plan plan);
}
public class PlanRepositorio : IPlanRepositorio
{
    private readonly GimnasioContext _context;
    public PlanRepositorio(GimnasioContext context)
    {
        _context = context;
    }
    public async Task<List<Plan>> ListarAsync(bool? activo, decimal? precioMax)
    {
        var query = _context.Planes
            .AsNoTracking()
            .Include(p => p.Socios)
            .AsQueryable();
        if (activo.HasValue)
            query = query.Where(p => p.Activo == activo.Value);
        if (precioMax.HasValue)
            query = query.Where(p => p.Precio <= precioMax.Value);
        return await query.OrderBy(p => p.Precio).ToListAsync();
    }
    public async Task<Plan?> ObtenerPorIdAsync(int id) =>
        await _context.Planes
            .AsNoTracking()
            .Include(p => p.Socios)
            .FirstOrDefaultAsync(p => p.PlanId == id);
    public async Task<Plan?> ObtenerParaEditarAsync(int id) =>
        await _context.Planes.FindAsync(id);
    public async Task<bool> ExisteAsync(int id) =>
        await _context.Planes.AnyAsync(p => p.PlanId == id);
    public async Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null) =>
        await _context.Planes.AnyAsync(p => p.Nombre == nombre && p.PlanId != excluirId);
    public async Task<bool> TieneSociosAsync(int id) =>
        await _context.Socios.AnyAsync(s => s.PlanId == id);
    public async Task AgregarAsync(Plan plan)
    {
        _context.Planes.Add(plan);
        await _context.SaveChangesAsync();
    }
    public async Task ActualizarAsync(Plan plan)
    {
        _context.Planes.Update(plan);
        await _context.SaveChangesAsync();
    }
    public async Task EliminarAsync(Plan plan)
    {
        _context.Planes.Remove(plan);
        await _context.SaveChangesAsync();
    }
}
