using System.ComponentModel.DataAnnotations;
namespace Gimnasio.Entidades.Dtos;
public class SocioDto
{
    public int SocioId { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int Edad { get; set; }
    public DateOnly FechaInscripcion { get; set; }
    public int PlanId { get; set; }
    public string Plan { get; set; } = string.Empty;
}
public class SocioCrearDto
{
    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Los nombres deben tener entre 3 y 100 caracteres.")]
    public string Nombres { get; set; } = string.Empty;
    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI solo debe contener números.")]
    public string Dni { get; set; } = string.Empty;
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
    [StringLength(100)]
    public string? Email { get; set; }
    [Range(14, 90, ErrorMessage = "La edad debe estar entre 14 y 90 años.")]
    public int Edad { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "El PlanId es obligatorio.")]
    public int PlanId { get; set; }
}
public class SocioActualizarDto
{
    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Los nombres deben tener entre 3 y 100 caracteres.")]
    public string Nombres { get; set; } = string.Empty;
    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI solo debe contener números.")]
    public string Dni { get; set; } = string.Empty;
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
    [StringLength(100)]
    public string? Email { get; set; }
    [Range(14, 90, ErrorMessage = "La edad debe estar entre 14 y 90 años.")]
    public int Edad { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "El PlanId es obligatorio.")]
    public int PlanId { get; set; }
}
