using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonasABM.Application.DTOs;
using PersonasABM.Application.Interfaces;

namespace PersonasABM.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PersonasController : ControllerBase
{
    private readonly IPersonaService _personaService;
    private readonly ILogger<PersonasController> _logger;

    public PersonasController(IPersonaService personaService, ILogger<PersonasController> logger)
    {
        _personaService = personaService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las personas
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonaDto>>> GetAll()
    {
        try
        {
            var personas = await _personaService.GetAllAsync();
            return Ok(personas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las personas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene una persona por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonaDto>> GetById(int id)
    {
        try
        {
            var persona = await _personaService.GetByIdAsync(id);
            if (persona == null)
            {
                return NotFound($"No se encontró la persona con ID: {id}");
            }
            return Ok(persona);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener persona con ID: {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea una nueva persona (Solo Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PersonaDto>> Create([FromBody] CreatePersonaDto createPersonaDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var persona = await _personaService.CreateAsync(createPersonaDto);
            return CreatedAtAction(nameof(GetById), new { id = persona.Id }, persona);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear persona");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza una persona existente (Solo Admin)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PersonaDto>> Update(int id, [FromBody] UpdatePersonaDto updatePersonaDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var persona = await _personaService.UpdateAsync(id, updatePersonaDto);
            return Ok(persona);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar persona con ID: {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina una persona (Solo Admin)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _personaService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar persona con ID: {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Busca personas con filtros
    /// </summary>
    [HttpPost("search")]
    public async Task<ActionResult<IEnumerable<PersonaDto>>> Search([FromBody] PersonaSearchDto searchDto)
    {
        try
        {
            var personas = await _personaService.SearchAsync(searchDto);
            return Ok(personas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar personas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Valida si una identificación está disponible
    /// </summary>
    [HttpGet("validate-identificacion/{identificacion}")]
    public async Task<ActionResult<bool>> ValidateIdentificacion(string identificacion, [FromQuery] int? excludeId = null)
    {
        try
        {
            var isValid = await _personaService.ValidateIdentificacionAsync(identificacion, excludeId);
            return Ok(new { isValid });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar identificación: {Identificacion}", identificacion);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
