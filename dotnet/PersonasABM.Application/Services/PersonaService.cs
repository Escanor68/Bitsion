using System.Text.Json;
using Microsoft.Extensions.Logging;
using PersonasABM.Application.DTOs;
using PersonasABM.Application.Interfaces;
using PersonasABM.Domain.Entities;

namespace PersonasABM.Application.Services;

public class PersonaService : IPersonaService
{
    private readonly IPersonaRepository _personaRepository;
    private readonly ILogger<PersonaService> _logger;

    public PersonaService(IPersonaRepository personaRepository, ILogger<PersonaService> logger)
    {
        _personaRepository = personaRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<PersonaDto>> GetAllAsync()
    {
        _logger.LogInformation("Obteniendo todas las personas");
        var personas = await _personaRepository.GetAllAsync();
        return personas.Select(MapToDto);
    }

    public async Task<PersonaDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Obteniendo persona con ID: {Id}", id);
        var persona = await _personaRepository.GetByIdAsync(id);
        return persona != null ? MapToDto(persona) : null;
    }

    public async Task<PersonaDto> CreateAsync(CreatePersonaDto createPersonaDto)
    {
        _logger.LogInformation("Creando nueva persona: {Identificacion}", createPersonaDto.Identificacion);
        
        // Validar que la identificación sea única
        if (await _personaRepository.ExistsByIdentificacionAsync(createPersonaDto.Identificacion))
        {
            throw new InvalidOperationException("Ya existe una persona con esta identificación");
        }

        var persona = new Persona
        {
            NombreCompleto = createPersonaDto.NombreCompleto,
            Identificacion = createPersonaDto.Identificacion,
            Edad = createPersonaDto.Edad,
            Genero = createPersonaDto.Genero,
            Estado = createPersonaDto.Estado,
            AtributosAdicionales = JsonSerializer.Serialize(createPersonaDto.AtributosAdicionales)
        };

        var createdPersona = await _personaRepository.AddAsync(persona);
        _logger.LogInformation("Persona creada exitosamente con ID: {Id}", createdPersona.Id);
        
        return MapToDto(createdPersona);
    }

    public async Task<PersonaDto> UpdateAsync(int id, UpdatePersonaDto updatePersonaDto)
    {
        _logger.LogInformation("Actualizando persona con ID: {Id}", id);
        
        var persona = await _personaRepository.GetByIdAsync(id);
        if (persona == null)
        {
            throw new KeyNotFoundException($"No se encontró la persona con ID: {id}");
        }

        // Validar que la identificación sea única (excluyendo el registro actual)
        if (await _personaRepository.ExistsByIdentificacionAsync(updatePersonaDto.Identificacion, id))
        {
            throw new InvalidOperationException("Ya existe otra persona con esta identificación");
        }

        persona.NombreCompleto = updatePersonaDto.NombreCompleto;
        persona.Identificacion = updatePersonaDto.Identificacion;
        persona.Edad = updatePersonaDto.Edad;
        persona.Genero = updatePersonaDto.Genero;
        persona.Estado = updatePersonaDto.Estado;
        persona.AtributosAdicionales = JsonSerializer.Serialize(updatePersonaDto.AtributosAdicionales);
        persona.FechaModificacion = DateTime.UtcNow;

        var updatedPersona = await _personaRepository.UpdateAsync(persona);
        _logger.LogInformation("Persona actualizada exitosamente con ID: {Id}", updatedPersona.Id);
        
        return MapToDto(updatedPersona);
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation("Eliminando persona con ID: {Id}", id);
        
        var persona = await _personaRepository.GetByIdAsync(id);
        if (persona == null)
        {
            throw new KeyNotFoundException($"No se encontró la persona con ID: {id}");
        }

        await _personaRepository.DeleteAsync(id);
        _logger.LogInformation("Persona eliminada exitosamente con ID: {Id}", id);
    }

    public async Task<IEnumerable<PersonaDto>> SearchAsync(PersonaSearchDto searchDto)
    {
        _logger.LogInformation("Buscando personas con filtros: {SearchDto}", JsonSerializer.Serialize(searchDto));
        
        var personas = await _personaRepository.SearchAsync(
            searchDto.Nombre, 
            searchDto.Estado, 
            searchDto.EdadMinima, 
            searchDto.EdadMaxima);
        
        return personas.Select(MapToDto);
    }

    public async Task<bool> ValidateIdentificacionAsync(string identificacion, int? excludeId = null)
    {
        return !await _personaRepository.ExistsByIdentificacionAsync(identificacion, excludeId);
    }

    private static PersonaDto MapToDto(Persona persona)
    {
        var atributosAdicionales = new Dictionary<string, object>();
        try
        {
            if (!string.IsNullOrEmpty(persona.AtributosAdicionales))
            {
                atributosAdicionales = JsonSerializer.Deserialize<Dictionary<string, object>>(persona.AtributosAdicionales) ?? new();
            }
        }
        catch
        {
            // Si hay error al deserializar, usar diccionario vacío
        }

        return new PersonaDto
        {
            Id = persona.Id,
            NombreCompleto = persona.NombreCompleto,
            Identificacion = persona.Identificacion,
            Edad = persona.Edad,
            Genero = persona.Genero,
            Estado = persona.Estado,
            FechaCreacion = persona.FechaCreacion,
            FechaModificacion = persona.FechaModificacion,
            AtributosAdicionales = atributosAdicionales,
            AtributosDetallados = persona.PersonaAtributos.Select(pa => new PersonaAtributoDto
            {
                Id = pa.Id,
                AtributoTipoId = pa.AtributoTipoId,
                NombreAtributo = pa.AtributoTipo.Nombre,
                TipoDato = pa.AtributoTipo.TipoDato,
                Valor = pa.Valor,
                EsObligatorio = pa.AtributoTipo.EsObligatorio
            }).ToList()
        };
    }
}
