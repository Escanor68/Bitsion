import { Persona } from '../entities/Persona';

export interface IPersonaRepository {
  findAll(): Promise<Persona[]>;
  findById(id: number): Promise<Persona | null>;
  findByIdentificacion(identificacion: string): Promise<Persona | null>;
  search(nombre?: string, estado?: string, edadMinima?: number, edadMaxima?: number): Promise<Persona[]>;
  create(persona: Persona): Promise<Persona>;
  update(persona: Persona): Promise<Persona>;
  delete(id: number): Promise<void>;
  existsByIdentificacion(identificacion: string, excludeId?: number): Promise<boolean>;
}
