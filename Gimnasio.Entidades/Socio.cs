namespace Gimnasio.Entidades;
public partial class Socio
{
    public int SocioId { get; set; }
    public string Nombres { get; set; } = null!;
    public string Dni { get; set; } = null!;
    public string? Email { get; set; }
    public int Edad { get; set; }
    public DateOnly FechaInscripcion { get; set; }
    public int PlanId { get; set; }
    public virtual Plan Plan { get; set; } = null!;
}
