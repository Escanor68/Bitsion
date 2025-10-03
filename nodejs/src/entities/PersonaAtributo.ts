import { Entity, PrimaryGeneratedColumn, Column, CreateDateColumn, UpdateDateColumn, ManyToOne, JoinColumn } from 'typeorm';
import { Persona } from './Persona';
import { AtributoTipo } from './AtributoTipo';

@Entity('persona_atributos')
export class PersonaAtributo {
  @PrimaryGeneratedColumn()
  id: number;

  @Column()
  personaId: number;

  @Column()
  atributoTipoId: number;

  @Column({ type: 'varchar', length: 500 })
  valor: string;

  @CreateDateColumn()
  fechaCreacion: Date;

  @UpdateDateColumn({ nullable: true })
  fechaModificacion?: Date;

  @ManyToOne(() => Persona, persona => persona.personaAtributos, { onDelete: 'CASCADE' })
  @JoinColumn({ name: 'personaId' })
  persona: Persona;

  @ManyToOne(() => AtributoTipo, atributoTipo => atributoTipo.personaAtributos, { onDelete: 'CASCADE' })
  @JoinColumn({ name: 'atributoTipoId' })
  atributoTipo: AtributoTipo;
}
