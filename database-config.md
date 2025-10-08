# Configuración de Base de Datos - Personas ABM

## Esquema Unificado

Ambas implementaciones (.NET y Node.js) utilizan el mismo esquema de base de datos con las siguientes tablas:

### Tablas Principales

1. **personas**

   - `Id` (PK, int, auto-increment)
   - `NombreCompleto` (varchar(200), required)
   - `Identificacion` (varchar(20), required, unique)
   - `Edad` (int, required)
   - `Genero` (varchar(10), required)
   - `Estado` (varchar(20), default: 'Activo')
   - `FechaCreacion` (datetime, auto-generated)
   - `FechaModificacion` (datetime, nullable)
   - `AtributosAdicionales` (json/text, default: '{}')

2. **atributo_tipos**

   - `Id` (PK, int, auto-increment)
   - `Nombre` (varchar(100), required)
   - `Descripcion` (varchar(500), nullable)
   - `TipoDato` (varchar(20), default: 'Texto')
   - `EsObligatorio` (bit/int, default: false)
   - `Activo` (bit/int, default: true)
   - `FechaCreacion` (datetime, auto-generated)

3. **persona_atributos**
   - `Id` (PK, int, auto-increment)
   - `PersonaId` (FK to personas.Id, cascade delete)
   - `AtributoTipoId` (FK to atributo_tipos.Id, cascade delete)
   - `Valor` (varchar(500), required)
   - `FechaCreacion` (datetime, auto-generated)
   - `FechaModificacion` (datetime, nullable)

## Configuración por Base de Datos

### SQL Server

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PersonasABM;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### MySQL

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=PersonasABM;Uid=personas_user;Pwd=personas123;"
  }
}
```

### SQLite

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=PersonasABM.db"
  }
}
```

## Scripts de Inicialización

- `schema.sql` - Script genérico con comentarios de compatibilidad
- `schema-mysql.sql` - Script específico para MySQL
- `schema-sqlite.sql` - Script específico para SQLite

## Variables de Entorno (Node.js)

```bash
# MySQL
DB_TYPE=mysql
DB_HOST=localhost
DB_PORT=3306
DB_USERNAME=personas_user
DB_PASSWORD=personas123
DB_DATABASE=PersonasABM

# SQLite
DB_TYPE=sqlite
DB_DATABASE=personas_abm_nodejs.db
```

## Migraciones

### .NET (EF Core)

```bash
dotnet ef migrations add InitialCreate --project PersonasABM.Infrastructure --startup-project PersonasABM.API
dotnet ef database update --project PersonasABM.Infrastructure --startup-project PersonasABM.API
```

### Node.js (TypeORM)

```bash
npm run migration:generate
npm run migration:run
```

## Datos de Prueba

Ambos proyectos incluyen los mismos datos de prueba:

- 4 tipos de atributos predefinidos
- 2 personas de ejemplo con atributos JSON
- Configuración de roles JWT (admin/consultor)
