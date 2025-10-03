import { DataSource } from 'typeorm';
import { Persona } from '../entities/Persona';
import { AtributoTipo } from '../entities/AtributoTipo';
import { PersonaAtributo } from '../entities/PersonaAtributo';

export const AppDataSource = new DataSource({
  type: 'mssql',
  host: process.env.DB_HOST || 'localhost',
  port: parseInt(process.env.DB_PORT || '1433'),
  username: process.env.DB_USERNAME || 'sa',
  password: process.env.DB_PASSWORD || 'YourPassword123',
  database: process.env.DB_DATABASE || 'PersonasABM',
  options: {
    encrypt: process.env.DB_ENCRYPT === 'true',
    trustServerCertificate: process.env.DB_TRUST_SERVER_CERTIFICATE === 'true',
  },
  entities: [Persona, AtributoTipo, PersonaAtributo],
  synchronize: process.env.NODE_ENV === 'development', // Solo para desarrollo
  logging: process.env.NODE_ENV === 'development',
  migrations: ['src/migrations/*.ts'],
  migrationsTableName: 'migrations',
});
