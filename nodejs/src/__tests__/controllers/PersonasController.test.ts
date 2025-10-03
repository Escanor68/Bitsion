import request from 'supertest';
import express from 'express';
import { PersonasController } from '../../controllers/PersonasController';
import { IPersonaService } from '../../services/IPersonaService';
import { PersonaDto, CreatePersonaDto, UpdatePersonaDto } from '../../dto/PersonaDto';

describe('PersonasController', () => {
  let app: express.Application;
  let mockPersonaService: jest.Mocked<IPersonaService>;
  let personasController: PersonasController;

  beforeEach(() => {
    mockPersonaService = {
      findAll: jest.fn(),
      findById: jest.fn(),
      create: jest.fn(),
      update: jest.fn(),
      delete: jest.fn(),
      search: jest.fn(),
      validateIdentificacion: jest.fn()
    } as jest.Mocked<IPersonaService>;

    personasController = new PersonasController(mockPersonaService);

    app = express();
    app.use(express.json());
    app.get('/personas', personasController.getAll.bind(personasController));
    app.get('/personas/:id', personasController.getById.bind(personasController));
    app.post('/personas', personasController.create.bind(personasController));
    app.put('/personas/:id', personasController.update.bind(personasController));
    app.delete('/personas/:id', personasController.delete.bind(personasController));
    app.post('/personas/search', personasController.search.bind(personasController));
    app.get('/personas/validate-identificacion/:identificacion', personasController.validateIdentificacion.bind(personasController));
  });

  describe('GET /personas', () => {
    it('should return all personas', async () => {
      const personas: PersonaDto[] = [
        {
          id: 1,
          nombreCompleto: 'Juan Pérez',
          identificacion: '12345678',
          edad: 30,
          genero: 'Masculino',
          estado: 'Activo',
          fechaCreacion: new Date(),
          atributosAdicionales: {},
          atributosDetallados: []
        }
      ];

      mockPersonaService.findAll.mockResolvedValue(personas);

      const response = await request(app)
        .get('/personas')
        .expect(200);

      expect(response.body).toEqual(personas);
      expect(mockPersonaService.findAll).toHaveBeenCalledTimes(1);
    });

    it('should handle service errors', async () => {
      mockPersonaService.findAll.mockRejectedValue(new Error('Database error'));

      const response = await request(app)
        .get('/personas')
        .expect(500);

      expect(response.body.message).toBe('Error interno del servidor');
    });
  });

  describe('GET /personas/:id', () => {
    it('should return persona when found', async () => {
      const persona: PersonaDto = {
        id: 1,
        nombreCompleto: 'Juan Pérez',
        identificacion: '12345678',
        edad: 30,
        genero: 'Masculino',
        estado: 'Activo',
        fechaCreacion: new Date(),
        atributosAdicionales: {},
        atributosDetallados: []
      };

      mockPersonaService.findById.mockResolvedValue(persona);

      const response = await request(app)
        .get('/personas/1')
        .expect(200);

      expect(response.body).toEqual(persona);
      expect(mockPersonaService.findById).toHaveBeenCalledWith(1);
    });

    it('should return 404 when persona not found', async () => {
      mockPersonaService.findById.mockResolvedValue(null);

      const response = await request(app)
        .get('/personas/999')
        .expect(404);

      expect(response.body.message).toBe('No se encontró la persona con ID: 999');
    });
  });

  describe('POST /personas', () => {
    it('should create persona with valid data', async () => {
      const createDto: CreatePersonaDto = {
        nombreCompleto: 'Carlos García',
        identificacion: '11223344',
        edad: 35,
        genero: 'Masculino',
        estado: 'Activo'
      };

      const createdPersona: PersonaDto = {
        id: 3,
        ...createDto,
        fechaCreacion: new Date(),
        atributosAdicionales: {},
        atributosDetallados: []
      };

      mockPersonaService.create.mockResolvedValue(createdPersona);

      const response = await request(app)
        .post('/personas')
        .send(createDto)
        .expect(201);

      expect(response.body).toEqual(createdPersona);
      expect(mockPersonaService.create).toHaveBeenCalledWith(createDto);
    });

    it('should return 400 when identificacion already exists', async () => {
      const createDto: CreatePersonaDto = {
        nombreCompleto: 'Carlos García',
        identificacion: '12345678',
        edad: 35,
        genero: 'Masculino',
        estado: 'Activo'
      };

      mockPersonaService.create.mockRejectedValue(new Error('Ya existe una persona con esta identificación'));

      const response = await request(app)
        .post('/personas')
        .send(createDto)
        .expect(400);

      expect(response.body.message).toBe('Ya existe una persona con esta identificación');
    });
  });

  describe('PUT /personas/:id', () => {
    it('should update persona with valid data', async () => {
      const updateDto: UpdatePersonaDto = {
        nombreCompleto: 'Juan Pérez García',
        identificacion: '12345678',
        edad: 31,
        genero: 'Masculino',
        estado: 'Activo'
      };

      const updatedPersona: PersonaDto = {
        id: 1,
        ...updateDto,
        fechaCreacion: new Date(),
        fechaModificacion: new Date(),
        atributosAdicionales: {},
        atributosDetallados: []
      };

      mockPersonaService.update.mockResolvedValue(updatedPersona);

      const response = await request(app)
        .put('/personas/1')
        .send(updateDto)
        .expect(200);

      expect(response.body).toEqual(updatedPersona);
      expect(mockPersonaService.update).toHaveBeenCalledWith(1, updateDto);
    });

    it('should return 404 when persona not found', async () => {
      const updateDto: UpdatePersonaDto = {
        nombreCompleto: 'Juan Pérez García',
        identificacion: '12345678',
        edad: 31,
        genero: 'Masculino',
        estado: 'Activo'
      };

      mockPersonaService.update.mockRejectedValue(new Error('No se encontró la persona con ID: 999'));

      const response = await request(app)
        .put('/personas/999')
        .send(updateDto)
        .expect(404);

      expect(response.body.message).toBe('No se encontró la persona con ID: 999');
    });
  });

  describe('DELETE /personas/:id', () => {
    it('should delete persona when found', async () => {
      mockPersonaService.delete.mockResolvedValue();

      await request(app)
        .delete('/personas/1')
        .expect(204);

      expect(mockPersonaService.delete).toHaveBeenCalledWith(1);
    });

    it('should return 404 when persona not found', async () => {
      mockPersonaService.delete.mockRejectedValue(new Error('No se encontró la persona con ID: 999'));

      const response = await request(app)
        .delete('/personas/999')
        .expect(404);

      expect(response.body.message).toBe('No se encontró la persona con ID: 999');
    });
  });

  describe('POST /personas/search', () => {
    it('should search personas with filters', async () => {
      const personas: PersonaDto[] = [
        {
          id: 1,
          nombreCompleto: 'Juan Pérez',
          identificacion: '12345678',
          edad: 30,
          genero: 'Masculino',
          estado: 'Activo',
          fechaCreacion: new Date(),
          atributosAdicionales: {},
          atributosDetallados: []
        }
      ];

      const searchDto = {
        nombre: 'Juan',
        estado: 'Activo',
        edadMinima: 25,
        edadMaxima: 35
      };

      mockPersonaService.search.mockResolvedValue(personas);

      const response = await request(app)
        .post('/personas/search')
        .send(searchDto)
        .expect(200);

      expect(response.body).toEqual(personas);
      expect(mockPersonaService.search).toHaveBeenCalledWith(searchDto);
    });
  });

  describe('GET /personas/validate-identificacion/:identificacion', () => {
    it('should return validation result', async () => {
      mockPersonaService.validateIdentificacion.mockResolvedValue(true);

      const response = await request(app)
        .get('/personas/validate-identificacion/12345678')
        .expect(200);

      expect(response.body).toEqual({ isValid: true });
      expect(mockPersonaService.validateIdentificacion).toHaveBeenCalledWith('12345678', undefined);
    });

    it('should exclude current persona when updating', async () => {
      mockPersonaService.validateIdentificacion.mockResolvedValue(true);

      const response = await request(app)
        .get('/personas/validate-identificacion/12345678?excludeId=1')
        .expect(200);

      expect(response.body).toEqual({ isValid: true });
      expect(mockPersonaService.validateIdentificacion).toHaveBeenCalledWith('12345678', 1);
    });
  });
});
