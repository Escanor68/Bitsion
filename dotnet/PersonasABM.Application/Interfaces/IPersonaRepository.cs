using PersonasABM.Domain.Entities;

namespace PersonasABM.Application.Interfaces;

public interface IPersonaRepository
{
    Task<IEnumerable<Persona>> GetAllAsync();
    Task<Persona?> GetByIdAsync(int id);
    Task<Persona?> GetByIdentificacionAsync(string identificacion);
    Task<IEnumerable<Persona>> SearchAsync(string? nombre, string? estado, int? edadMinima, int? edadMaxima);
    Task<Persona> AddAsync(Persona persona);
    Task<Persona> UpdateAsync(Persona persona);
    Task DeleteAsync(int id);
    Task<bool> ExistsByIdentificacionAsync(string identificacion, int? excludeId = null);
}
