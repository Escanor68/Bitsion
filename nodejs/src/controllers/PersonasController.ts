import { Request, Response } from 'express';
import { IPersonaService } from '../services/IPersonaService';
import { PersonaSearchDto } from '../dto/PersonaDto';
import { AuthRequest } from '../middleware/auth';

export class PersonasController {
  constructor(private personaService: IPersonaService) {}

  async getAll(req: AuthRequest, res: Response): Promise<void> {
    try {
      const personas = await this.personaService.findAll();
      res.json(personas);
    } catch (error) {
      console.error('Error al obtener todas las personas:', error);
      res.status(500).json({ message: 'Error interno del servidor' });
    }
  }

  async getById(req: AuthRequest, res: Response): Promise<void> {
    try {
      const id = parseInt(req.params.id);
      const persona = await this.personaService.findById(id);
      
      if (!persona) {
        res.status(404).json({ message: `No se encontró la persona con ID: ${id}` });
        return;
      }
      
      res.json(persona);
    } catch (error) {
      console.error('Error al obtener persona:', error);
      res.status(500).json({ message: 'Error interno del servidor' });
    }
  }

  async create(req: AuthRequest, res: Response): Promise<void> {
    try {
      const persona = await this.personaService.create(req.body);
      res.status(201).json(persona);
    } catch (error: any) {
      console.error('Error al crear persona:', error);
      
      if (error.message.includes('Ya existe')) {
        res.status(400).json({ message: error.message });
        return;
      }
      
      res.status(500).json({ message: 'Error interno del servidor' });
    }
  }

  async update(req: AuthRequest, res: Response): Promise<void> {
    try {
      const id = parseInt(req.params.id);
      const persona = await this.personaService.update(id, req.body);
      res.json(persona);
    } catch (error: any) {
      console.error('Error al actualizar persona:', error);
      
      if (error.message.includes('No se encontró')) {
        res.status(404).json({ message: error.message });
        return;
      }
      
      if (error.message.includes('Ya existe')) {
        res.status(400).json({ message: error.message });
        return;
      }
      
      res.status(500).json({ message: 'Error interno del servidor' });
    }
  }

  async delete(req: AuthRequest, res: Response): Promise<void> {
    try {
      const id = parseInt(req.params.id);
      await this.personaService.delete(id);
      res.status(204).send();
    } catch (error: any) {
      console.error('Error al eliminar persona:', error);
      
      if (error.message.includes('No se encontró')) {
        res.status(404).json({ message: error.message });
        return;
      }
      
      res.status(500).json({ message: 'Error interno del servidor' });
    }
  }

  async search(req: AuthRequest, res: Response): Promise<void> {
    try {
      const searchDto: PersonaSearchDto = req.body;
      const personas = await this.personaService.search(searchDto);
      res.json(personas);
    } catch (error) {
      console.error('Error al buscar personas:', error);
      res.status(500).json({ message: 'Error interno del servidor' });
    }
  }

  async validateIdentificacion(req: AuthRequest, res: Response): Promise<void> {
    try {
      const { identificacion } = req.params;
      const excludeId = req.query.excludeId ? parseInt(req.query.excludeId as string) : undefined;
      
      const isValid = await this.personaService.validateIdentificacion(identificacion, excludeId);
      res.json({ isValid });
    } catch (error) {
      console.error('Error al validar identificación:', error);
      res.status(500).json({ message: 'Error interno del servidor' });
    }
  }
}
