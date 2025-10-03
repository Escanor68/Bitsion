using Microsoft.EntityFrameworkCore;
using PersonasABM.Application.Interfaces;
using PersonasABM.Domain.Entities;
using PersonasABM.Infrastructure.Data;

namespace PersonasABM.Infrastructure.Repositories;

public class PersonaRepository : IPersonaRepository
{
    private readonly ApplicationDbContext _context;

    public PersonaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Persona>> GetAllAsync()
    {
        return await _context.Personas
            .Include(p => p.PersonaAtributos)
            .ThenInclude(pa => pa.AtributoTipo)
            .ToListAsync();
    }

    public async Task<Persona?> GetByIdAsync(int id)
    {
        return await _context.Personas
            .Include(p => p.PersonaAtributos)
            .ThenInclude(pa => pa.AtributoTipo)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Persona?> GetByIdentificacionAsync(string identificacion)
    {
        return await _context.Personas
            .Include(p => p.PersonaAtributos)
            .ThenInclude(pa => pa.AtributoTipo)
            .FirstOrDefaultAsync(p => p.Identificacion == identificacion);
    }

    public async Task<IEnumerable<Persona>> SearchAsync(string? nombre, string? estado, int? edadMinima, int? edadMaxima)
    {
        var query = _context.Personas
            .Include(p => p.PersonaAtributos)
            .ThenInclude(pa => pa.AtributoTipo)
            .AsQueryable();

        if (!string.IsNullOrEmpty(nombre))
        {
            query = query.Where(p => p.NombreCompleto.Contains(nombre));
        }

        if (!string.IsNullOrEmpty(estado))
        {
            query = query.Where(p => p.Estado == estado);
        }

        if (edadMinima.HasValue)
        {
            query = query.Where(p => p.Edad >= edadMinima.Value);
        }

        if (edadMaxima.HasValue)
        {
            query = query.Where(p => p.Edad <= edadMaxima.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Persona> AddAsync(Persona persona)
    {
        _context.Personas.Add(persona);
        await _context.SaveChangesAsync();
        return persona;
    }

    public async Task<Persona> UpdateAsync(Persona persona)
    {
        _context.Personas.Update(persona);
        await _context.SaveChangesAsync();
        return persona;
    }

    public async Task DeleteAsync(int id)
    {
        var persona = await _context.Personas.FindAsync(id);
        if (persona != null)
        {
            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByIdentificacionAsync(string identificacion, int? excludeId = null)
    {
        var query = _context.Personas.Where(p => p.Identificacion == identificacion);
        
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}
