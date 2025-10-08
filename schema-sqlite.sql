-- =============================================
-- Script de inicialización para SQLite
-- Personas ABM - Esquema unificado
-- =============================================

-- Tabla: personas
CREATE TABLE personas (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreCompleto TEXT NOT NULL,
    Identificacion TEXT NOT NULL UNIQUE,
    Edad INTEGER NOT NULL,
    Genero TEXT NOT NULL,
    Estado TEXT NOT NULL DEFAULT 'Activo',
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FechaModificacion DATETIME NULL,
    AtributosAdicionales TEXT NOT NULL DEFAULT '{}'
);

-- Tabla: atributo_tipos
CREATE TABLE atributo_tipos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Descripcion TEXT NULL,
    TipoDato TEXT NOT NULL DEFAULT 'Texto',
    EsObligatorio INTEGER NOT NULL DEFAULT 0,
    Activo INTEGER NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Tabla: persona_atributos
CREATE TABLE persona_atributos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PersonaId INTEGER NOT NULL,
    AtributoTipoId INTEGER NOT NULL,
    Valor TEXT NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FechaModificacion DATETIME NULL,
    
    -- Foreign Keys
    FOREIGN KEY (PersonaId) REFERENCES personas(Id) ON DELETE CASCADE,
    FOREIGN KEY (AtributoTipoId) REFERENCES atributo_tipos(Id) ON DELETE CASCADE
);

-- Índices para optimización
CREATE INDEX IX_personas_Identificacion ON personas(Identificacion);
CREATE INDEX IX_persona_atributos_PersonaId ON persona_atributos(PersonaId);
CREATE INDEX IX_persona_atributos_AtributoTipoId ON persona_atributos(AtributoTipoId);

-- Datos de prueba
INSERT INTO atributo_tipos (Nombre, Descripcion, TipoDato, EsObligatorio, Activo, FechaCreacion) VALUES
('Maneja', '¿La persona maneja vehículos?', 'Booleano', 0, 1, CURRENT_TIMESTAMP),
('Usa Lentes', '¿La persona usa lentes correctivos?', 'Booleano', 0, 1, CURRENT_TIMESTAMP),
('Es Diabético', '¿La persona tiene diabetes?', 'Booleano', 0, 1, CURRENT_TIMESTAMP),
('Tipo de Sangre', 'Tipo de sangre de la persona', 'Texto', 0, 1, CURRENT_TIMESTAMP);

INSERT INTO personas (NombreCompleto, Identificacion, Edad, Genero, Estado, FechaCreacion, AtributosAdicionales) VALUES
('Juan Pérez García', '12345678', 35, 'Masculino', 'Activo', CURRENT_TIMESTAMP, '{"Maneja": true, "Usa Lentes": false, "Es Diabético": false, "Tipo de Sangre": "O+"}'),
('María López Rodríguez', '87654321', 28, 'Femenino', 'Activo', CURRENT_TIMESTAMP, '{"Maneja": true, "Usa Lentes": true, "Es Diabético": true, "Tipo de Sangre": "A+"}');
