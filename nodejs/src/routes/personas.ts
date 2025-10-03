import { Router } from 'express';
import { PersonasController } from '../controllers/PersonasController';
import { authenticateToken, requireRole } from '../middleware/auth';
import { IPersonaService } from '../services/IPersonaService';

const router = Router();

export const createPersonasRouter = (personaService: IPersonaService) => {
  const personasController = new PersonasController(personaService);

  // Aplicar autenticación a todas las rutas
  router.use(authenticateToken);

  // Rutas de solo lectura (Admin y Consultor)
  router.get('/', personasController.getAll.bind(personasController));
  router.get('/:id', personasController.getById.bind(personasController));
  router.post('/search', personasController.search.bind(personasController));
  router.get('/validate-identificacion/:identificacion', personasController.validateIdentificacion.bind(personasController));

  // Rutas de escritura (Solo Admin)
  router.post('/', requireRole(['Admin']), personasController.create.bind(personasController));
  router.put('/:id', requireRole(['Admin']), personasController.update.bind(personasController));
  router.delete('/:id', requireRole(['Admin']), personasController.delete.bind(personasController));

  return router;
};
