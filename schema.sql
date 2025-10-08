-- =============================================
-- Script de inicialización unificado para Personas ABM
-- Compatible con SQL Server, MySQL y SQLite
-- =============================================

-- Tabla: personas
CREATE TABLE personas (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- SQL Server
    -- Id INT AUTO_INCREMENT PRIMARY KEY,  -- MySQL
    -- Id INTEGER PRIMARY KEY AUTOINCREMENT,  -- SQLite
    NombreCompleto NVARCHAR(200) NOT NULL,
    Identificacion NVARCHAR(20) NOT NULL UNIQUE,
    Edad INT NOT NULL,
    Genero NVARCHAR(10) NOT NULL,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Activo',
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaModificacion DATETIME2 NULL,
    AtributosAdicionales NVARCHAR(MAX) NOT NULL DEFAULT '{}'
);

-- Tabla: atributo_tipos
CREATE TABLE atributo_tipos (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- SQL Server
    -- Id INT AUTO_INCREMENT PRIMARY KEY,  -- MySQL
    -- Id INTEGER PRIMARY KEY AUTOINCREMENT,  -- SQLite
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(500) NULL,
    TipoDato NVARCHAR(20) NOT NULL DEFAULT 'Texto',
    EsObligatorio BIT NOT NULL DEFAULT 0,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Tabla: persona_atributos
CREATE TABLE persona_atributos (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- SQL Server
    -- Id INT AUTO_INCREMENT PRIMARY KEY,  -- MySQL
    -- Id INTEGER PRIMARY KEY AUTOINCREMENT,  -- SQLite
    PersonaId INT NOT NULL,
    AtributoTipoId INT NOT NULL,
    Valor NVARCHAR(500) NOT NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaModificacion DATETIME2 NULL,
    
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
('Maneja', '¿La persona maneja vehículos?', 'Booleano', 0, 1, GETUTCDATE()),
('Usa Lentes', '¿La persona usa lentes correctivos?', 'Booleano', 0, 1, GETUTCDATE()),
('Es Diabético', '¿La persona tiene diabetes?', 'Booleano', 0, 1, GETUTCDATE()),
('Tipo de Sangre', 'Tipo de sangre de la persona', 'Texto', 0, 1, GETUTCDATE());

INSERT INTO personas (NombreCompleto, Identificacion, Edad, Genero, Estado, FechaCreacion, AtributosAdicionales) VALUES
('Juan Pérez García', '12345678', 35, 'Masculino', 'Activo', GETUTCDATE(), '{"Maneja": true, "Usa Lentes": false, "Es Diabético": false, "Tipo de Sangre": "O+"}'),
('María López Rodríguez', '87654321', 28, 'Femenino', 'Activo', GETUTCDATE(), '{"Maneja": true, "Usa Lentes": true, "Es Diabético": true, "Tipo de Sangre": "A+"}');

-- =============================================
-- Notas de compatibilidad:
-- =============================================
-- SQL Server: Usar IDENTITY(1,1) y DATETIME2
-- MySQL: Usar AUTO_INCREMENT y DATETIME
-- SQLite: Usar AUTOINCREMENT y DATETIME
-- 
-- Para MySQL, cambiar:
-- - IDENTITY(1,1) por AUTO_INCREMENT
-- - DATETIME2 por DATETIME
-- - GETUTCDATE() por NOW()
-- - NVARCHAR por VARCHAR
-- - BIT por TINYINT(1)
-- 
-- Para SQLite, cambiar:
-- - IDENTITY(1,1) por AUTOINCREMENT
-- - DATETIME2 por DATETIME
-- - GETUTCDATE() por CURRENT_TIMESTAMP
-- - NVARCHAR por TEXT
-- - BIT por INTEGER
