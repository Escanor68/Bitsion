import { PersonaDto, CreatePersonaDto, UpdatePersonaDto, PersonaSearchDto } from '../dto/PersonaDto';

export interface IPersonaService {
  findAll(): Promise<PersonaDto[]>;
  findById(id: number): Promise<PersonaDto | null>;
  create(createPersonaDto: CreatePersonaDto): Promise<PersonaDto>;
  update(id: number, updatePersonaDto: UpdatePersonaDto): Promise<PersonaDto>;
  delete(id: number): Promise<void>;
  search(searchDto: PersonaSearchDto): Promise<PersonaDto[]>;
  validateIdentificacion(identificacion: string, excludeId?: number): Promise<boolean>;
}
