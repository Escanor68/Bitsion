using Microsoft.EntityFrameworkCore;
using PersonasABM.Domain.Entities;

namespace PersonasABM.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Persona> Personas { get; set; }
    public DbSet<AtributoTipo> AtributoTipos { get; set; }
    public DbSet<PersonaAtributo> PersonaAtributos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Persona
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombreCompleto).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Identificacion).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Genero).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Estado).IsRequired().HasMaxLength(20);
            entity.Property(e => e.AtributosAdicionales).HasColumnType("nvarchar(max)");
            
            entity.HasIndex(e => e.Identificacion).IsUnique();
        });

        // Configuración de AtributoTipo
        modelBuilder.Entity<AtributoTipo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.TipoDato).IsRequired().HasMaxLength(20);
        });

        // Configuración de PersonaAtributo
        modelBuilder.Entity<PersonaAtributo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Valor).IsRequired().HasMaxLength(500);
            
            entity.HasOne(e => e.Persona)
                  .WithMany(p => p.PersonaAtributos)
                  .HasForeignKey(e => e.PersonaId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.AtributoTipo)
                  .WithMany(at => at.PersonaAtributos)
                  .HasForeignKey(e => e.AtributoTipoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Datos de prueba
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Atributos de prueba
        modelBuilder.Entity<AtributoTipo>().HasData(
            new AtributoTipo
            {
                Id = 1,
                Nombre = "Maneja",
                Descripcion = "¿La persona maneja vehículos?",
                TipoDato = "Booleano",
                EsObligatorio = false,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            },
            new AtributoTipo
            {
                Id = 2,
                Nombre = "Usa Lentes",
                Descripcion = "¿La persona usa lentes correctivos?",
                TipoDato = "Booleano",
                EsObligatorio = false,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            },
            new AtributoTipo
            {
                Id = 3,
                Nombre = "Es Diabético",
                Descripcion = "¿La persona tiene diabetes?",
                TipoDato = "Booleano",
                EsObligatorio = false,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            },
            new AtributoTipo
            {
                Id = 4,
                Nombre = "Tipo de Sangre",
                Descripcion = "Tipo de sangre de la persona",
                TipoDato = "Texto",
                EsObligatorio = false,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            }
        );

        // Personas de prueba
        modelBuilder.Entity<Persona>().HasData(
            new Persona
            {
                Id = 1,
                NombreCompleto = "Juan Pérez García",
                Identificacion = "12345678",
                Edad = 35,
                Genero = "Masculino",
                Estado = "Activo",
                FechaCreacion = DateTime.UtcNow,
                AtributosAdicionales = "{\"Maneja\": true, \"Usa Lentes\": false, \"Es Diabético\": false, \"Tipo de Sangre\": \"O+\"}"
            },
            new Persona
            {
                Id = 2,
                NombreCompleto = "María López Rodríguez",
                Identificacion = "87654321",
                Edad = 28,
                Genero = "Femenino",
                Estado = "Activo",
                FechaCreacion = DateTime.UtcNow,
                AtributosAdicionales = "{\"Maneja\": true, \"Usa Lentes\": true, \"Es Diabético\": true, \"Tipo de Sangre\": \"A+\"}"
            }
        );
    }
}
