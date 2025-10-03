namespace PersonasABM.Application.DTOs;

public class PersonaSearchDto
{
    public string? Nombre { get; set; }
    public string? Estado { get; set; }
    public int? EdadMinima { get; set; }
    public int? EdadMaxima { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
