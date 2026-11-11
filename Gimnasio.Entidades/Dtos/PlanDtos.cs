using System.ComponentModel.DataAnnotations;
namespace Gimnasio.Entidades.Dtos;
public class PlanDto
{
    public int PlanId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int DuracionMeses { get; set; }
    public bool Activo { get; set; }
    public int CantidadSocios { get; set; }
}
public class PlanCrearDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
    [Range(1.0, 9999.99, ErrorMessage = "El precio debe estar entre 1 y 9999.99.")]
    public decimal Precio { get; set; }
    [Range(1, 24, ErrorMessage = "La duración debe estar entre 1 y 24 meses.")]
    public int DuracionMeses { get; set; }
    public bool Activo { get; set; } = true;
}
public class PlanActualizarDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
    [Range(1.0, 9999.99, ErrorMessage = "El precio debe estar entre 1 y 9999.99.")]
    public decimal Precio { get; set; }
    [Range(1, 24, ErrorMessage = "La duración debe estar entre 1 y 24 meses.")]
    public int DuracionMeses { get; set; }
    public bool Activo { get; set; }
}
