import { MigrationInterface, QueryRunner } from "typeorm";

export class InitialCreate1759951957111 implements MigrationInterface {
    name = 'InitialCreate1759951957111'

    public async up(queryRunner: QueryRunner): Promise<void> {
        await queryRunner.query(`CREATE TABLE "atributo_tipos" ("id" integer PRIMARY KEY AUTOINCREMENT NOT NULL, "nombre" varchar(100) NOT NULL, "descripcion" varchar(500), "tipoDato" varchar(20) NOT NULL DEFAULT ('Texto'), "esObligatorio" boolean NOT NULL DEFAULT (0), "activo" boolean NOT NULL DEFAULT (1), "fechaCreacion" datetime NOT NULL DEFAULT (datetime('now')))`);
        await queryRunner.query(`CREATE TABLE "persona_atributos" ("id" integer PRIMARY KEY AUTOINCREMENT NOT NULL, "personaId" integer NOT NULL, "atributoTipoId" integer NOT NULL, "valor" varchar(500) NOT NULL, "fechaCreacion" datetime NOT NULL DEFAULT (datetime('now')), "fechaModificacion" datetime DEFAULT (datetime('now')))`);
        await queryRunner.query(`CREATE TABLE "personas" ("id" integer PRIMARY KEY AUTOINCREMENT NOT NULL, "nombreCompleto" varchar(200) NOT NULL, "identificacion" varchar(20) NOT NULL, "edad" integer NOT NULL, "genero" varchar(10) NOT NULL, "estado" varchar(20) NOT NULL DEFAULT ('Activo'), "atributosAdicionales" text NOT NULL DEFAULT ('{}'), "fechaCreacion" datetime NOT NULL DEFAULT (datetime('now')), "fechaModificacion" datetime DEFAULT (datetime('now')), CONSTRAINT "UQ_8e46d893cdb0d3ca0435ae165fb" UNIQUE ("identificacion"))`);
        await queryRunner.query(`CREATE TABLE "temporary_persona_atributos" ("id" integer PRIMARY KEY AUTOINCREMENT NOT NULL, "personaId" integer NOT NULL, "atributoTipoId" integer NOT NULL, "valor" varchar(500) NOT NULL, "fechaCreacion" datetime NOT NULL DEFAULT (datetime('now')), "fechaModificacion" datetime DEFAULT (datetime('now')), CONSTRAINT "FK_0120dc4dcd9144702f8b8d800fd" FOREIGN KEY ("personaId") REFERENCES "personas" ("id") ON DELETE CASCADE ON UPDATE NO ACTION, CONSTRAINT "FK_a7ae3491c03572ecb4f5339d4f1" FOREIGN KEY ("atributoTipoId") REFERENCES "atributo_tipos" ("id") ON DELETE CASCADE ON UPDATE NO ACTION)`);
        await queryRunner.query(`INSERT INTO "temporary_persona_atributos"("id", "personaId", "atributoTipoId", "valor", "fechaCreacion", "fechaModificacion") SELECT "id", "personaId", "atributoTipoId", "valor", "fechaCreacion", "fechaModificacion" FROM "persona_atributos"`);
        await queryRunner.query(`DROP TABLE "persona_atributos"`);
        await queryRunner.query(`ALTER TABLE "temporary_persona_atributos" RENAME TO "persona_atributos"`);
        await queryRunner.query(`CREATE TABLE "query-result-cache" ("id" integer PRIMARY KEY AUTOINCREMENT NOT NULL, "identifier" varchar, "time" bigint NOT NULL, "duration" integer NOT NULL, "query" text NOT NULL, "result" text NOT NULL)`);
    }

    public async down(queryRunner: QueryRunner): Promise<void> {
        await queryRunner.query(`DROP TABLE "query-result-cache"`);
        await queryRunner.query(`ALTER TABLE "persona_atributos" RENAME TO "temporary_persona_atributos"`);
        await queryRunner.query(`CREATE TABLE "persona_atributos" ("id" integer PRIMARY KEY AUTOINCREMENT NOT NULL, "personaId" integer NOT NULL, "atributoTipoId" integer NOT NULL, "valor" varchar(500) NOT NULL, "fechaCreacion" datetime NOT NULL DEFAULT (datetime('now')), "fechaModificacion" datetime DEFAULT (datetime('now')))`);
        await queryRunner.query(`INSERT INTO "persona_atributos"("id", "personaId", "atributoTipoId", "valor", "fechaCreacion", "fechaModificacion") SELECT "id", "personaId", "atributoTipoId", "valor", "fechaCreacion", "fechaModificacion" FROM "temporary_persona_atributos"`);
        await queryRunner.query(`DROP TABLE "temporary_persona_atributos"`);
        await queryRunner.query(`DROP TABLE "personas"`);
        await queryRunner.query(`DROP TABLE "persona_atributos"`);
        await queryRunner.query(`DROP TABLE "atributo_tipos"`);
    }

}
