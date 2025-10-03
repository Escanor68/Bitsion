using PersonasABM.Application.DTOs;

namespace PersonasABM.Application.Interfaces;

public interface IPersonaService
{
    Task<IEnumerable<PersonaDto>> GetAllAsync();
    Task<PersonaDto?> GetByIdAsync(int id);
    Task<PersonaDto> CreateAsync(CreatePersonaDto createPersonaDto);
    Task<PersonaDto> UpdateAsync(int id, UpdatePersonaDto updatePersonaDto);
    Task DeleteAsync(int id);
    Task<IEnumerable<PersonaDto>> SearchAsync(PersonaSearchDto searchDto);
    Task<bool> ValidateIdentificacionAsync(string identificacion, int? excludeId = null);
}
