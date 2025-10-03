using System.ComponentModel.DataAnnotations;

namespace PersonasABM.Domain.Entities;

public class PersonaAtributo
{
    public int Id { get; set; }
    
    public int PersonaId { get; set; }
    
    public int AtributoTipoId { get; set; }
    
    [Required]
    [StringLength(500)]
    public string Valor { get; set; } = string.Empty;
    
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    
    public DateTime? FechaModificacion { get; set; }
    
    // Navegación
    public virtual Persona Persona { get; set; } = null!;
    public virtual AtributoTipo AtributoTipo { get; set; } = null!;
}
