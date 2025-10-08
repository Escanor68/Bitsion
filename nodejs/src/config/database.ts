import { DataSource } from 'typeorm';
import { Persona } from '../entities/Persona';
import { AtributoTipo } from '../entities/AtributoTipo';
import { PersonaAtributo } from '../entities/PersonaAtributo';

export const AppDataSource = new DataSource({
  type: 'mysql',
  host: process.env.DB_HOST || 'localhost',
  port: parseInt(process.env.DB_PORT || '3306'),
  username: process.env.DB_USERNAME || 'personas_user',
  password: process.env.DB_PASSWORD || 'personas123',
  database: process.env.DB_DATABASE || 'PersonasABM',
  entities: [Persona, AtributoTipo, PersonaAtributo],
  synchronize: process.env.NODE_ENV === 'development', // Solo para desarrollo
  logging: process.env.NODE_ENV === 'development',
  migrations: ['src/migrations/*.ts'],
  migrationsTableName: 'migrations',
});
