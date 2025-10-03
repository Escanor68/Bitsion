namespace PersonasABM.Application.DTOs;

public class PersonaDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    public int Edad { get; set; }
    public string Genero { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public Dictionary<string, object> AtributosAdicionales { get; set; } = new();
    public List<PersonaAtributoDto> AtributosDetallados { get; set; } = new();
}

public class PersonaAtributoDto
{
    public int Id { get; set; }
    public int AtributoTipoId { get; set; }
    public string NombreAtributo { get; set; } = string.Empty;
    public string TipoDato { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public bool EsObligatorio { get; set; }
}
