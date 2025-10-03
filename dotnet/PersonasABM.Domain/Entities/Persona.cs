using System.ComponentModel.DataAnnotations;

namespace PersonasABM.Domain.Entities;

public class Persona
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string NombreCompleto { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    public string Identificacion { get; set; } = string.Empty;
    
    [Range(1, 150)]
    public int Edad { get; set; }
    
    [Required]
    [StringLength(10)]
    public string Genero { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = "Activo";
    
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    
    public DateTime? FechaModificacion { get; set; }
    
    // Atributos adicionales como JSON
    public string AtributosAdicionales { get; set; } = "{}";
    
    // Navegación
    public virtual ICollection<PersonaAtributo> PersonaAtributos { get; set; } = new List<PersonaAtributo>();
}
