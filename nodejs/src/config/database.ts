import { DataSource } from 'typeorm';
import { Persona } from '../entities/Persona';
import { AtributoTipo } from '../entities/AtributoTipo';
import { PersonaAtributo } from '../entities/PersonaAtributo';

export const AppDataSource = new DataSource({
  type: 'sqlite',
  database: process.env.DB_DATABASE || './PersonasABM.db',
  entities: [Persona, AtributoTipo, PersonaAtributo],
  synchronize: false, // Deshabilitar synchronize para usar migraciones
  logging: process.env.NODE_ENV === 'development',
  migrations: ['dist/migrations/*.js'], // Usar archivos compilados
  migrationsTableName: 'migrations',
  // Configuraciones adicionales para SQLite
  enableWAL: true, // Habilitar Write-Ahead Logging para mejor rendimiento
  cache: {
    duration: 30000, // Cache de 30 segundos
  },
});

// Función para inicializar la conexión a la base de datos
export const initializeDatabase = async (): Promise<void> => {
  try {
    if (!AppDataSource.isInitialized) {
      await AppDataSource.initialize();
      console.log('✅ Base de datos SQLite conectada exitosamente');
      console.log(`📁 Ubicación: ${AppDataSource.options.database}`);
    }
  } catch (error) {
    console.error('❌ Error al conectar con la base de datos SQLite:', error);
    throw error;
  }
};
