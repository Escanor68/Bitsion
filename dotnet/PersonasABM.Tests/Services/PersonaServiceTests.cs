using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PersonasABM.Application.DTOs;
using PersonasABM.Application.Services;
using PersonasABM.Domain.Entities;
using PersonasABM.Application.Interfaces;

namespace PersonasABM.Tests.Services;

public class PersonaServiceTests
{
    private readonly Mock<IPersonaRepository> _mockRepository;
    private readonly Mock<ILogger<PersonaService>> _mockLogger;
    private readonly PersonaService _service;

    public PersonaServiceTests()
    {
        _mockRepository = new Mock<IPersonaRepository>();
        _mockLogger = new Mock<ILogger<PersonaService>>();
        _service = new PersonaService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPersonas()
    {
        // Arrange
        var personas = new List<Persona>
        {
            new Persona { Id = 1, NombreCompleto = "Juan Pérez", Identificacion = "12345678", Edad = 30, Genero = "Masculino", Estado = "Activo" },
            new Persona { Id = 2, NombreCompleto = "María López", Identificacion = "87654321", Edad = 25, Genero = "Femenino", Estado = "Activo" }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(personas);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.First().NombreCompleto.Should().Be("Juan Pérez");
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnPersona()
    {
        // Arrange
        var persona = new Persona 
        { 
            Id = 1, 
            NombreCompleto = "Juan Pérez", 
            Identificacion = "12345678", 
            Edad = 30, 
            Genero = "Masculino", 
            Estado = "Activo" 
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(persona);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.NombreCompleto.Should().Be("Juan Pérez");
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Persona?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreatePersona()
    {
        // Arrange
        var createDto = new CreatePersonaDto
        {
            NombreCompleto = "Carlos García",
            Identificacion = "11223344",
            Edad = 35,
            Genero = "Masculino",
            Estado = "Activo"
        };

        var createdPersona = new Persona
        {
            Id = 3,
            NombreCompleto = createDto.NombreCompleto,
            Identificacion = createDto.Identificacion,
            Edad = createDto.Edad,
            Genero = createDto.Genero,
            Estado = createDto.Estado
        };

        _mockRepository.Setup(r => r.ExistsByIdentificacionAsync(createDto.Identificacion)).ReturnsAsync(false);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Persona>())).ReturnsAsync(createdPersona);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.NombreCompleto.Should().Be(createDto.NombreCompleto);
        result.Identificacion.Should().Be(createDto.Identificacion);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateIdentificacion_ShouldThrowException()
    {
        // Arrange
        var createDto = new CreatePersonaDto
        {
            NombreCompleto = "Carlos García",
            Identificacion = "12345678", // Duplicada
            Edad = 35,
            Genero = "Masculino",
            Estado = "Activo"
        };

        _mockRepository.Setup(r => r.ExistsByIdentificacionAsync(createDto.Identificacion)).ReturnsAsync(true);

        // Act & Assert
        await _service.Invoking(s => s.CreateAsync(createDto))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Ya existe una persona con esta identificación");
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdatePersona()
    {
        // Arrange
        var existingPersona = new Persona
        {
            Id = 1,
            NombreCompleto = "Juan Pérez",
            Identificacion = "12345678",
            Edad = 30,
            Genero = "Masculino",
            Estado = "Activo"
        };

        var updateDto = new UpdatePersonaDto
        {
            NombreCompleto = "Juan Pérez García",
            Identificacion = "12345678",
            Edad = 31,
            Genero = "Masculino",
            Estado = "Activo"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingPersona);
        _mockRepository.Setup(r => r.ExistsByIdentificacionAsync(updateDto.Identificacion, 1)).ReturnsAsync(false);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Persona>())).ReturnsAsync(existingPersona);

        // Act
        var result = await _service.UpdateAsync(1, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.NombreCompleto.Should().Be(updateDto.NombreCompleto);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentId_ShouldThrowException()
    {
        // Arrange
        var updateDto = new UpdatePersonaDto
        {
            NombreCompleto = "Juan Pérez García",
            Identificacion = "12345678",
            Edad = 31,
            Genero = "Masculino",
            Estado = "Activo"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Persona?)null);

        // Act & Assert
        await _service.Invoking(s => s.UpdateAsync(999, updateDto))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("No se encontró la persona con ID: 999");
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldDeletePersona()
    {
        // Arrange
        var existingPersona = new Persona
        {
            Id = 1,
            NombreCompleto = "Juan Pérez",
            Identificacion = "12345678",
            Edad = 30,
            Genero = "Masculino",
            Estado = "Activo"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingPersona);
        _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistentId_ShouldThrowException()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Persona?)null);

        // Act & Assert
        await _service.Invoking(s => s.DeleteAsync(999))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("No se encontró la persona con ID: 999");
    }

    [Fact]
    public async Task SearchAsync_WithFilters_ShouldReturnFilteredResults()
    {
        // Arrange
        var personas = new List<Persona>
        {
            new Persona { Id = 1, NombreCompleto = "Juan Pérez", Edad = 30, Estado = "Activo" },
            new Persona { Id = 2, NombreCompleto = "María López", Edad = 25, Estado = "Activo" }
        };

        var searchDto = new PersonaSearchDto
        {
            Nombre = "Juan",
            Estado = "Activo",
            EdadMinima = 25,
            EdadMaxima = 35
        };

        _mockRepository.Setup(r => r.SearchAsync("Juan", "Activo", 25, 35)).ReturnsAsync(personas);

        // Act
        var result = await _service.SearchAsync(searchDto);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task ValidateIdentificacionAsync_WithAvailableIdentificacion_ShouldReturnTrue()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsByIdentificacionAsync("12345678", null)).ReturnsAsync(false);

        // Act
        var result = await _service.ValidateIdentificacionAsync("12345678");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateIdentificacionAsync_WithTakenIdentificacion_ShouldReturnFalse()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsByIdentificacionAsync("12345678", null)).ReturnsAsync(true);

        // Act
        var result = await _service.ValidateIdentificacionAsync("12345678");

        // Assert
        result.Should().BeFalse();
    }
}
