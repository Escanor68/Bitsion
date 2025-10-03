using System.ComponentModel.DataAnnotations;

namespace PersonasABM.Domain.Entities;

public class AtributoTipo
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Descripcion { get; set; }
    
    [Required]
    [StringLength(20)]
    public string TipoDato { get; set; } = "Texto"; // Texto, Booleano, Numero, Fecha
    
    public bool EsObligatorio { get; set; } = false;
    
    public bool Activo { get; set; } = true;
    
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    
    // Navegación
    public virtual ICollection<PersonaAtributo> PersonaAtributos { get; set; } = new List<PersonaAtributo>();
}
