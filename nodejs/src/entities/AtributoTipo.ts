import {
  Entity,
  PrimaryGeneratedColumn,
  Column,
  CreateDateColumn,
  OneToMany,
} from 'typeorm';
import { PersonaAtributo } from './PersonaAtributo';

@Entity('atributo_tipos')
export class AtributoTipo {
  @PrimaryGeneratedColumn()
  id!: number;

  @Column({ type: 'varchar', length: 100 })
  nombre!: string;

  @Column({ type: 'varchar', length: 500, nullable: true })
  descripcion?: string;

  @Column({ type: 'varchar', length: 20, default: 'Texto' })
  tipoDato!: string;

  @Column({ type: 'boolean', default: false })
  esObligatorio!: boolean;

  @Column({ type: 'boolean', default: true })
  activo!: boolean;

  @CreateDateColumn()
  fechaCreacion!: Date;

  @OneToMany(
    () => PersonaAtributo,
    (personaAtributo) => personaAtributo.atributoTipo
  )
  personaAtributos!: PersonaAtributo[];
}
