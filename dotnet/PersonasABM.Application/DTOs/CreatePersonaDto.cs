using System.ComponentModel.DataAnnotations;

namespace PersonasABM.Application.DTOs;

public class CreatePersonaDto
{
    [Required(ErrorMessage = "El nombre completo es obligatorio")]
    [StringLength(200, ErrorMessage = "El nombre completo no puede exceder 200 caracteres")]
    public string NombreCompleto { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La identificación es obligatoria")]
    [StringLength(20, ErrorMessage = "La identificación no puede exceder 20 caracteres")]
    public string Identificacion { get; set; } = string.Empty;
    
    [Range(1, 150, ErrorMessage = "La edad debe estar entre 1 y 150 años")]
    public int Edad { get; set; }
    
    [Required(ErrorMessage = "El género es obligatorio")]
    [StringLength(10, ErrorMessage = "El género no puede exceder 10 caracteres")]
    public string Genero { get; set; } = string.Empty;
    
    [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
    public string Estado { get; set; } = "Activo";
    
    public Dictionary<string, object> AtributosAdicionales { get; set; } = new();
}

public class UpdatePersonaDto
{
    [Required(ErrorMessage = "El nombre completo es obligatorio")]
    [StringLength(200, ErrorMessage = "El nombre completo no puede exceder 200 caracteres")]
    public string NombreCompleto { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La identificación es obligatoria")]
    [StringLength(20, ErrorMessage = "La identificación no puede exceder 20 caracteres")]
    public string Identificacion { get; set; } = string.Empty;
    
    [Range(1, 150, ErrorMessage = "La edad debe estar entre 1 y 150 años")]
    public int Edad { get; set; }
    
    [Required(ErrorMessage = "El género es obligatorio")]
    [StringLength(10, ErrorMessage = "El género no puede exceder 10 caracteres")]
    public string Genero { get; set; } = string.Empty;
    
    [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
    public string Estado { get; set; } = string.Empty;
    
    public Dictionary<string, object> AtributosAdicionales { get; set; } = new();
}
