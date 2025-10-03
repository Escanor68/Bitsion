import { PersonaDto, CreatePersonaDto, UpdatePersonaDto, PersonaSearchDto } from '../dto/PersonaDto';
import { PersonaAtributoDto } from '../dto/PersonaAtributoDto';
import { IPersonaService } from './IPersonaService';
import { IPersonaRepository } from '../repositories/IPersonaRepository';
import { Persona } from '../entities/Persona';

export class PersonaService implements IPersonaService {
  constructor(private personaRepository: IPersonaRepository) {}

  async findAll(): Promise<PersonaDto[]> {
    const personas = await this.personaRepository.findAll();
    return personas.map(this.mapToDto);
  }

  async findById(id: number): Promise<PersonaDto | null> {
    const persona = await this.personaRepository.findById(id);
    return persona ? this.mapToDto(persona) : null;
  }

  async create(createPersonaDto: CreatePersonaDto): Promise<PersonaDto> {
    // Validar que la identificación sea única
    if (await this.personaRepository.existsByIdentificacion(createPersonaDto.identificacion)) {
      throw new Error('Ya existe una persona con esta identificación');
    }

    const persona = new Persona();
    persona.nombreCompleto = createPersonaDto.nombreCompleto;
    persona.identificacion = createPersonaDto.identificacion;
    persona.edad = createPersonaDto.edad;
    persona.genero = createPersonaDto.genero;
    persona.estado = createPersonaDto.estado || 'Activo';
    persona.atributosAdicionales = JSON.stringify(createPersonaDto.atributosAdicionales || {});

    const createdPersona = await this.personaRepository.create(persona);
    return this.mapToDto(createdPersona);
  }

  async update(id: number, updatePersonaDto: UpdatePersonaDto): Promise<PersonaDto> {
    const persona = await this.personaRepository.findById(id);
    if (!persona) {
      throw new Error(`No se encontró la persona con ID: ${id}`);
    }

    // Validar que la identificación sea única (excluyendo el registro actual)
    if (await this.personaRepository.existsByIdentificacion(updatePersonaDto.identificacion, id)) {
      throw new Error('Ya existe otra persona con esta identificación');
    }

    persona.nombreCompleto = updatePersonaDto.nombreCompleto;
    persona.identificacion = updatePersonaDto.identificacion;
    persona.edad = updatePersonaDto.edad;
    persona.genero = updatePersonaDto.genero;
    persona.estado = updatePersonaDto.estado;
    persona.atributosAdicionales = JSON.stringify(updatePersonaDto.atributosAdicionales || {});

    const updatedPersona = await this.personaRepository.update(persona);
    return this.mapToDto(updatedPersona);
  }

  async delete(id: number): Promise<void> {
    const persona = await this.personaRepository.findById(id);
    if (!persona) {
      throw new Error(`No se encontró la persona con ID: ${id}`);
    }

    await this.personaRepository.delete(id);
  }

  async search(searchDto: PersonaSearchDto): Promise<PersonaDto[]> {
    const personas = await this.personaRepository.search(
      searchDto.nombre,
      searchDto.estado,
      searchDto.edadMinima,
      searchDto.edadMaxima
    );
    return personas.map(this.mapToDto);
  }

  async validateIdentificacion(identificacion: string, excludeId?: number): Promise<boolean> {
    return !(await this.personaRepository.existsByIdentificacion(identificacion, excludeId));
  }

  private mapToDto(persona: Persona): PersonaDto {
    let atributosAdicionales: Record<string, any> = {};
    try {
      if (persona.atributosAdicionales) {
        atributosAdicionales = JSON.parse(persona.atributosAdicionales);
      }
    } catch {
      // Si hay error al parsear, usar objeto vacío
    }

    return {
      id: persona.id,
      nombreCompleto: persona.nombreCompleto,
      identificacion: persona.identificacion,
      edad: persona.edad,
      genero: persona.genero,
      estado: persona.estado,
      fechaCreacion: persona.fechaCreacion,
      fechaModificacion: persona.fechaModificacion,
      atributosAdicionales,
      atributosDetallados: persona.personaAtributos?.map(pa => ({
        id: pa.id,
        atributoTipoId: pa.atributoTipoId,
        nombreAtributo: pa.atributoTipo?.nombre || '',
        tipoDato: pa.atributoTipo?.tipoDato || '',
        valor: pa.valor,
        esObligatorio: pa.atributoTipo?.esObligatorio || false
      })) || []
    };
  }
}
