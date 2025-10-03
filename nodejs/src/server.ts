import 'reflect-metadata';
import express from 'express';
import cors from 'cors';
import helmet from 'helmet';
import morgan from 'morgan';
import compression from 'compression';
import rateLimit from 'express-rate-limit';
import dotenv from 'dotenv';
import { AppDataSource } from './config/database';
import { PersonaRepository } from './repositories/PersonaRepository';
import { PersonaService } from './services/PersonaService';
import authRoutes from './routes/auth';
import { createPersonasRouter } from './routes/personas';

// Cargar variables de entorno
dotenv.config();

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware de seguridad
app.use(helmet());
app.use(cors());
app.use(compression());

// Rate limiting
const limiter = rateLimit({
  windowMs: 15 * 60 * 1000, // 15 minutos
  max: 100, // máximo 100 requests por IP por ventana
  message: 'Demasiadas solicitudes desde esta IP, intenta de nuevo más tarde.'
});
app.use(limiter);

// Middleware de logging
app.use(morgan('combined'));

// Middleware para parsear JSON
app.use(express.json({ limit: '10mb' }));
app.use(express.urlencoded({ extended: true }));

// Rutas
app.use('/api/auth', authRoutes);

// Inicializar servicios
const personaRepository = new PersonaRepository();
const personaService = new PersonaService(personaRepository);
app.use('/api/personas', createPersonasRouter(personaService));

// Ruta de salud
app.get('/health', (req, res) => {
  res.json({ 
    status: 'OK', 
    timestamp: new Date().toISOString(),
    uptime: process.uptime()
  });
});

// Ruta raíz con información de la API
app.get('/', (req, res) => {
  res.json({
    message: 'Personas ABM API',
    version: '1.0.0',
    documentation: '/api-docs',
    health: '/health'
  });
});

// Middleware de manejo de errores
app.use((err: any, req: express.Request, res: express.Response, next: express.NextFunction) => {
  console.error('Error no manejado:', err);
  res.status(500).json({ 
    message: 'Error interno del servidor',
    error: process.env.NODE_ENV === 'development' ? err.message : undefined
  });
});

// Middleware para rutas no encontradas
app.use('*', (req, res) => {
  res.status(404).json({ message: 'Ruta no encontrada' });
});

// Inicializar base de datos y servidor
async function startServer() {
  try {
    // Inicializar TypeORM
    await AppDataSource.initialize();
    console.log('✅ Base de datos conectada exitosamente');

    // Iniciar servidor
    app.listen(PORT, () => {
      console.log(`🚀 Servidor ejecutándose en puerto ${PORT}`);
      console.log(`📚 Documentación disponible en http://localhost:${PORT}`);
      console.log(`🔍 Health check en http://localhost:${PORT}/health`);
    });
  } catch (error) {
    console.error('❌ Error al iniciar el servidor:', error);
    process.exit(1);
  }
}

// Manejo de señales de terminación
process.on('SIGINT', async () => {
  console.log('\n🛑 Cerrando servidor...');
  await AppDataSource.destroy();
  process.exit(0);
});

process.on('SIGTERM', async () => {
  console.log('\n🛑 Cerrando servidor...');
  await AppDataSource.destroy();
  process.exit(0);
});

startServer();
