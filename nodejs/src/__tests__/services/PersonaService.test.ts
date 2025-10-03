import { PersonaService } from '../../services/PersonaService';
import { IPersonaRepository } from '../../repositories/IPersonaRepository';
import { Persona } from '../../entities/Persona';
import { CreatePersonaDto, UpdatePersonaDto, PersonaSearchDto } from '../../dto/PersonaDto';

describe('PersonaService', () => {
  let personaService: PersonaService;
  let mockRepository: jest.Mocked<IPersonaRepository>;

  beforeEach(() => {
    mockRepository = {
      findAll: jest.fn(),
      findById: jest.fn(),
      findByIdentificacion: jest.fn(),
      search: jest.fn(),
      create: jest.fn(),
      update: jest.fn(),
      delete: jest.fn(),
      existsByIdentificacion: jest.fn()
    } as jest.Mocked<IPersonaRepository>;

    personaService = new PersonaService(mockRepository);
  });

  describe('findAll', () => {
    it('should return all personas', async () => {
      const personas = [
        createMockPersona(1, 'Juan Pérez', '12345678', 30, 'Masculino', 'Activo'),
        createMockPersona(2, 'María López', '87654321', 25, 'Femenino', 'Activo')
      ];

      mockRepository.findAll.mockResolvedValue(personas);

      const result = await personaService.findAll();

      expect(result).toHaveLength(2);
      expect(result[0].nombreCompleto).toBe('Juan Pérez');
      expect(mockRepository.findAll).toHaveBeenCalledTimes(1);
    });
  });

  describe('findById', () => {
    it('should return persona when found', async () => {
      const persona = createMockPersona(1, 'Juan Pérez', '12345678', 30, 'Masculino', 'Activo');
      mockRepository.findById.mockResolvedValue(persona);

      const result = await personaService.findById(1);

      expect(result).not.toBeNull();
      expect(result!.nombreCompleto).toBe('Juan Pérez');
      expect(mockRepository.findById).toHaveBeenCalledWith(1);
    });

    it('should return null when not found', async () => {
      mockRepository.findById.mockResolvedValue(null);

      const result = await personaService.findById(999);

      expect(result).toBeNull();
      expect(mockRepository.findById).toHaveBeenCalledWith(999);
    });
  });

  describe('create', () => {
    it('should create persona with valid data', async () => {
      const createDto: CreatePersonaDto = {
        nombreCompleto: 'Carlos García',
        identificacion: '11223344',
        edad: 35,
        genero: 'Masculino',
        estado: 'Activo'
      };

      const createdPersona = createMockPersona(3, createDto.nombreCompleto, createDto.identificacion, createDto.edad, createDto.genero, createDto.estado);

      mockRepository.existsByIdentificacion.mockResolvedValue(false);
      mockRepository.create.mockResolvedValue(createdPersona);

      const result = await personaService.create(createDto);

      expect(result.nombreCompleto).toBe(createDto.nombreCompleto);
      expect(result.identificacion).toBe(createDto.identificacion);
      expect(mockRepository.existsByIdentificacion).toHaveBeenCalledWith(createDto.identificacion);
      expect(mockRepository.create).toHaveBeenCalledTimes(1);
    });

    it('should throw error when identificacion already exists', async () => {
      const createDto: CreatePersonaDto = {
        nombreCompleto: 'Carlos García',
        identificacion: '12345678', // Duplicada
        edad: 35,
        genero: 'Masculino',
        estado: 'Activo'
      };

      mockRepository.existsByIdentificacion.mockResolvedValue(true);

      await expect(personaService.create(createDto))
        .rejects
        .toThrow('Ya existe una persona con esta identificación');
    });
  });

  describe('update', () => {
    it('should update persona with valid data', async () => {
      const existingPersona = createMockPersona(1, 'Juan Pérez', '12345678', 30, 'Masculino', 'Activo');
      const updateDto: UpdatePersonaDto = {
        nombreCompleto: 'Juan Pérez García',
        identificacion: '12345678',
        edad: 31,
        genero: 'Masculino',
        estado: 'Activo'
      };

      mockRepository.findById.mockResolvedValue(existingPersona);
      mockRepository.existsByIdentificacion.mockResolvedValue(false);
      mockRepository.update.mockResolvedValue({ ...existingPersona, ...updateDto });

      const result = await personaService.update(1, updateDto);

      expect(result.nombreCompleto).toBe(updateDto.nombreCompleto);
      expect(mockRepository.findById).toHaveBeenCalledWith(1);
      expect(mockRepository.existsByIdentificacion).toHaveBeenCalledWith(updateDto.identificacion, 1);
    });

    it('should throw error when persona not found', async () => {
      const updateDto: UpdatePersonaDto = {
        nombreCompleto: 'Juan Pérez García',
        identificacion: '12345678',
        edad: 31,
        genero: 'Masculino',
        estado: 'Activo'
      };

      mockRepository.findById.mockResolvedValue(null);

      await expect(personaService.update(999, updateDto))
        .rejects
        .toThrow('No se encontró la persona con ID: 999');
    });

    it('should throw error when identificacion already exists for another persona', async () => {
      const existingPersona = createMockPersona(1, 'Juan Pérez', '12345678', 30, 'Masculino', 'Activo');
      const updateDto: UpdatePersonaDto = {
        nombreCompleto: 'Juan Pérez García',
        identificacion: '87654321', // Ya existe para otra persona
        edad: 31,
        genero: 'Masculino',
        estado: 'Activo'
      };

      mockRepository.findById.mockResolvedValue(existingPersona);
      mockRepository.existsByIdentificacion.mockResolvedValue(true);

      await expect(personaService.update(1, updateDto))
        .rejects
        .toThrow('Ya existe otra persona con esta identificación');
    });
  });

  describe('delete', () => {
    it('should delete persona when found', async () => {
      const persona = createMockPersona(1, 'Juan Pérez', '12345678', 30, 'Masculino', 'Activo');
      mockRepository.findById.mockResolvedValue(persona);
      mockRepository.delete.mockResolvedValue();

      await personaService.delete(1);

      expect(mockRepository.findById).toHaveBeenCalledWith(1);
      expect(mockRepository.delete).toHaveBeenCalledWith(1);
    });

    it('should throw error when persona not found', async () => {
      mockRepository.findById.mockResolvedValue(null);

      await expect(personaService.delete(999))
        .rejects
        .toThrow('No se encontró la persona con ID: 999');
    });
  });

  describe('search', () => {
    it('should search personas with filters', async () => {
      const personas = [
        createMockPersona(1, 'Juan Pérez', '12345678', 30, 'Masculino', 'Activo'),
        createMockPersona(2, 'María López', '87654321', 25, 'Femenino', 'Activo')
      ];

      const searchDto: PersonaSearchDto = {
        nombre: 'Juan',
        estado: 'Activo',
        edadMinima: 25,
        edadMaxima: 35
      };

      mockRepository.search.mockResolvedValue(personas);

      const result = await personaService.search(searchDto);

      expect(result).toHaveLength(2);
      expect(mockRepository.search).toHaveBeenCalledWith('Juan', 'Activo', 25, 35);
    });
  });

  describe('validateIdentificacion', () => {
    it('should return true when identificacion is available', async () => {
      mockRepository.existsByIdentificacion.mockResolvedValue(false);

      const result = await personaService.validateIdentificacion('12345678');

      expect(result).toBe(true);
      expect(mockRepository.existsByIdentificacion).toHaveBeenCalledWith('12345678', undefined);
    });

    it('should return false when identificacion is taken', async () => {
      mockRepository.existsByIdentificacion.mockResolvedValue(true);

      const result = await personaService.validateIdentificacion('12345678');

      expect(result).toBe(false);
    });

    it('should exclude current persona when updating', async () => {
      mockRepository.existsByIdentificacion.mockResolvedValue(false);

      const result = await personaService.validateIdentificacion('12345678', 1);

      expect(result).toBe(true);
      expect(mockRepository.existsByIdentificacion).toHaveBeenCalledWith('12345678', 1);
    });
  });

  function createMockPersona(id: number, nombreCompleto: string, identificacion: string, edad: number, genero: string, estado: string): Persona {
    const persona = new Persona();
    persona.id = id;
    persona.nombreCompleto = nombreCompleto;
    persona.identificacion = identificacion;
    persona.edad = edad;
    persona.genero = genero;
    persona.estado = estado;
    persona.fechaCreacion = new Date();
    persona.atributosAdicionales = '{}';
    persona.personaAtributos = [];
    return persona;
  }
});
