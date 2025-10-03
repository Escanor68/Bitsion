import { PersonaAtributoDto } from './PersonaAtributoDto';

export interface PersonaDto {
  id: number;
  nombreCompleto: string;
  identificacion: string;
  edad: number;
  genero: string;
  estado: string;
  fechaCreacion: Date;
  fechaModificacion?: Date;
  atributosAdicionales: Record<string, any>;
  atributosDetallados: PersonaAtributoDto[];
}

export interface CreatePersonaDto {
  nombreCompleto: string;
  identificacion: string;
  edad: number;
  genero: string;
  estado?: string;
  atributosAdicionales?: Record<string, any>;
}

export interface UpdatePersonaDto {
  nombreCompleto: string;
  identificacion: string;
  edad: number;
  genero: string;
  estado: string;
  atributosAdicionales?: Record<string, any>;
}

export interface PersonaSearchDto {
  nombre?: string;
  estado?: string;
  edadMinima?: number;
  edadMaxima?: number;
  pageNumber?: number;
  pageSize?: number;
}
