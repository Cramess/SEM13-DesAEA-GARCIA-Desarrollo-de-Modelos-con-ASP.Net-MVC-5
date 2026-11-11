namespace Gimnasio.Entidades;
public partial class Plan
{
    public int PlanId { get; set; }
    public string Nombre { get; set; } = null!;
    public decimal Precio { get; set; }
    public int DuracionMeses { get; set; }
    public bool Activo { get; set; }
    public virtual ICollection<Socio> Socios { get; set; } = new List<Socio>();
}
