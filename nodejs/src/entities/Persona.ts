import { Entity, PrimaryGeneratedColumn, Column, CreateDateColumn, UpdateDateColumn, OneToMany } from 'typeorm';
import { PersonaAtributo } from './PersonaAtributo';

@Entity('personas')
export class Persona {
  @PrimaryGeneratedColumn()
  id: number;

  @Column({ type: 'varchar', length: 200 })
  nombreCompleto: string;

  @Column({ type: 'varchar', length: 20, unique: true })
  identificacion: string;

  @Column({ type: 'int' })
  edad: number;

  @Column({ type: 'varchar', length: 10 })
  genero: string;

  @Column({ type: 'varchar', length: 20, default: 'Activo' })
  estado: string;

  @Column({ type: 'nvarchar', length: 'MAX', default: '{}' })
  atributosAdicionales: string;

  @CreateDateColumn()
  fechaCreacion: Date;

  @UpdateDateColumn({ nullable: true })
  fechaModificacion?: Date;

  @OneToMany(() => PersonaAtributo, personaAtributo => personaAtributo.persona, { cascade: true })
  personaAtributos: PersonaAtributo[];
}
