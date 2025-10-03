import { Repository } from 'typeorm';
import { AppDataSource } from '../config/database';
import { Persona } from '../entities/Persona';
import { IPersonaRepository } from './IPersonaRepository';

export class PersonaRepository implements IPersonaRepository {
  private repository: Repository<Persona>;

  constructor() {
    this.repository = AppDataSource.getRepository(Persona);
  }

  async findAll(): Promise<Persona[]> {
    return await this.repository.find({
      relations: ['personaAtributos', 'personaAtributos.atributoTipo'],
      order: { fechaCreacion: 'DESC' }
    });
  }

  async findById(id: number): Promise<Persona | null> {
    return await this.repository.findOne({
      where: { id },
      relations: ['personaAtributos', 'personaAtributos.atributoTipo']
    });
  }

  async findByIdentificacion(identificacion: string): Promise<Persona | null> {
    return await this.repository.findOne({
      where: { identificacion },
      relations: ['personaAtributos', 'personaAtributos.atributoTipo']
    });
  }

  async search(nombre?: string, estado?: string, edadMinima?: number, edadMaxima?: number): Promise<Persona[]> {
    const queryBuilder = this.repository
      .createQueryBuilder('persona')
      .leftJoinAndSelect('persona.personaAtributos', 'personaAtributo')
      .leftJoinAndSelect('personaAtributo.atributoTipo', 'atributoTipo')
      .orderBy('persona.fechaCreacion', 'DESC');

    if (nombre) {
      queryBuilder.andWhere('persona.nombreCompleto LIKE :nombre', { nombre: `%${nombre}%` });
    }

    if (estado) {
      queryBuilder.andWhere('persona.estado = :estado', { estado });
    }

    if (edadMinima !== undefined) {
      queryBuilder.andWhere('persona.edad >= :edadMinima', { edadMinima });
    }

    if (edadMaxima !== undefined) {
      queryBuilder.andWhere('persona.edad <= :edadMaxima', { edadMaxima });
    }

    return await queryBuilder.getMany();
  }

  async create(persona: Persona): Promise<Persona> {
    return await this.repository.save(persona);
  }

  async update(persona: Persona): Promise<Persona> {
    persona.fechaModificacion = new Date();
    return await this.repository.save(persona);
  }

  async delete(id: number): Promise<void> {
    await this.repository.delete(id);
  }

  async existsByIdentificacion(identificacion: string, excludeId?: number): Promise<boolean> {
    const queryBuilder = this.repository
      .createQueryBuilder('persona')
      .where('persona.identificacion = :identificacion', { identificacion });

    if (excludeId) {
      queryBuilder.andWhere('persona.id != :excludeId', { excludeId });
    }

    const count = await queryBuilder.getCount();
    return count > 0;
  }
}
