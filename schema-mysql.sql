-- =============================================
-- Script de inicialización para MySQL
-- Personas ABM - Esquema unificado
-- =============================================

-- Tabla: personas
CREATE TABLE personas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    NombreCompleto VARCHAR(200) NOT NULL,
    Identificacion VARCHAR(20) NOT NULL UNIQUE,
    Edad INT NOT NULL,
    Genero VARCHAR(10) NOT NULL,
    Estado VARCHAR(20) NOT NULL DEFAULT 'Activo',
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FechaModificacion DATETIME NULL,
    AtributosAdicionales JSON NOT NULL DEFAULT ('{}')
);

-- Tabla: atributo_tipos
CREATE TABLE atributo_tipos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(500) NULL,
    TipoDato VARCHAR(20) NOT NULL DEFAULT 'Texto',
    EsObligatorio TINYINT(1) NOT NULL DEFAULT 0,
    Activo TINYINT(1) NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Tabla: persona_atributos
CREATE TABLE persona_atributos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PersonaId INT NOT NULL,
    AtributoTipoId INT NOT NULL,
    Valor VARCHAR(500) NOT NULL,
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
('Maneja', '¿La persona maneja vehículos?', 'Booleano', 0, 1, NOW()),
('Usa Lentes', '¿La persona usa lentes correctivos?', 'Booleano', 0, 1, NOW()),
('Es Diabético', '¿La persona tiene diabetes?', 'Booleano', 0, 1, NOW()),
('Tipo de Sangre', 'Tipo de sangre de la persona', 'Texto', 0, 1, NOW());

INSERT INTO personas (NombreCompleto, Identificacion, Edad, Genero, Estado, FechaCreacion, AtributosAdicionales) VALUES
('Juan Pérez García', '12345678', 35, 'Masculino', 'Activo', NOW(), JSON_OBJECT('Maneja', true, 'Usa Lentes', false, 'Es Diabético', false, 'Tipo de Sangre', 'O+')),
('María López Rodríguez', '87654321', 28, 'Femenino', 'Activo', NOW(), JSON_OBJECT('Maneja', true, 'Usa Lentes', true, 'Es Diabético', true, 'Tipo de Sangre', 'A+'));
