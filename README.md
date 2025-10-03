# Personas ABM - Ejercicio Técnico

Este repositorio contiene dos implementaciones completas del sistema de gestión de personas con atributos dinámicos:

- **Implementación .NET**: API REST con .NET 8, Entity Framework Core y SQL Server
- **Implementación Node.js**: API REST con TypeScript, TypeORM y SQL Server

## 📋 Requisitos Funcionales

- ✅ CRUD completo de personas (Crear, Leer, Actualizar, Eliminar)
- ✅ Búsqueda y filtrado por nombre, estado y edad (rango)
- ✅ Gestión de atributos adicionales dinámicos
- ✅ Validaciones básicas (campos obligatorios, edad válida, identificación única)
- ✅ Autenticación JWT con roles (Admin/Consultor)
- ✅ Logging y documentación API

## 🏗️ Arquitectura

### Estrategia de Atributos Dinámicos

Se implementó una **estrategia mixta** que combina:

1. **Columna JSON** para atributos variables rápidos
2. **Tabla pivote** para atributos estructurados y consultas complejas

**Ventajas de esta estrategia:**
- Flexibilidad para agregar nuevos atributos sin modificar esquema
- Performance optimizada para consultas simples (JSON)
- Capacidad de validaciones complejas y relaciones (tabla pivote)
- Escalabilidad y mantenibilidad

## 🚀 Implementaciones

### 1. Implementación .NET

**Tecnologías:**
- .NET 8
- Entity Framework Core
- SQL Server LocalDB
- JWT Authentication
- Swagger/OpenAPI
- xUnit Testing

**Estructura:**
```
dotnet/
├── PersonasABM.API/          # Capa de presentación
├── PersonasABM.Application/  # Capa de aplicación
├── PersonasABM.Domain/       # Capa de dominio
├── PersonasABM.Infrastructure/ # Capa de infraestructura
└── PersonasABM.Tests/        # Tests unitarios
```

### 2. Implementación Node.js

**Tecnologías:**
- Node.js + TypeScript
- TypeORM
- Express.js
- SQL Server
- JWT Authentication
- Jest Testing

**Estructura:**
```
nodejs/
├── src/
│   ├── entities/         # Entidades de dominio
│   ├── dto/             # Data Transfer Objects
│   ├── repositories/    # Capa de acceso a datos
│   ├── services/        # Lógica de negocio
│   ├── controllers/     # Controladores REST
│   ├── middleware/      # Middleware personalizado
│   ├── routes/          # Definición de rutas
│   └── config/          # Configuración
└── tests/               # Tests unitarios
```

## 🔧 Instalación y Configuración

### Prerrequisitos

- .NET 8 SDK (para implementación .NET)
- Node.js 18+ (para implementación Node.js)
- SQL Server o SQL Server LocalDB
- Git

### Implementación .NET

1. **Clonar y navegar al directorio:**
```bash
cd dotnet
```

2. **Restaurar dependencias:**
```bash
dotnet restore
```

3. **Configurar base de datos:**
```bash
dotnet ef database update --project PersonasABM.Infrastructure --startup-project PersonasABM.API
```

4. **Ejecutar la aplicación:**
```bash
dotnet run --project PersonasABM.API
```

5. **Acceder a la documentación:**
- Swagger UI: `http://localhost:5000`
- API Base: `http://localhost:5000/api`

### Implementación Node.js

1. **Clonar y navegar al directorio:**
```bash
cd nodejs
```

2. **Instalar dependencias:**
```bash
npm install
```

3. **Configurar variables de entorno:**
```bash
cp env.example .env
# Editar .env con tu configuración de base de datos
```

4. **Compilar TypeScript:**
```bash
npm run build
```

5. **Ejecutar migraciones:**
```bash
npm run migration:run
```

6. **Ejecutar la aplicación:**
```bash
# Desarrollo
npm run dev

# Producción
npm start
```

7. **Acceder a la API:**
- API Base: `http://localhost:3000`
- Health Check: `http://localhost:3000/health`

## 🔐 Autenticación

### Usuarios Demo

| Usuario | Contraseña | Rol | Permisos |
|---------|------------|-----|----------|
| admin | admin123 | Admin | CRUD completo |
| consultor | consultor123 | Consultor | Solo lectura |

### Obtener Token JWT

```bash
# .NET
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "admin123"}'

# Node.js
curl -X POST "http://localhost:3000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "admin123"}'
```

### Usar Token en Requests

```bash
curl -X GET "http://localhost:5000/api/personas" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## 📚 Endpoints de la API

### Autenticación
- `POST /api/auth/login` - Iniciar sesión

### Personas
- `GET /api/personas` - Listar todas las personas
- `GET /api/personas/{id}` - Obtener persona por ID
- `POST /api/personas` - Crear persona (Solo Admin)
- `PUT /api/personas/{id}` - Actualizar persona (Solo Admin)
- `DELETE /api/personas/{id}` - Eliminar persona (Solo Admin)
- `POST /api/personas/search` - Buscar personas con filtros
- `GET /api/personas/validate-identificacion/{identificacion}` - Validar identificación

### Ejemplo de Request/Response

**Crear Persona:**
```json
POST /api/personas
{
  "nombreCompleto": "Juan Pérez García",
  "identificacion": "12345678",
  "edad": 35,
  "genero": "Masculino",
  "estado": "Activo",
  "atributosAdicionales": {
    "Maneja": true,
    "Usa Lentes": false,
    "Es Diabético": false,
    "Tipo de Sangre": "O+"
  }
}
```

**Buscar Personas:**
```json
POST /api/personas/search
{
  "nombre": "Juan",
  "estado": "Activo",
  "edadMinima": 25,
  "edadMaxima": 50
}
```

## 🧪 Testing

### .NET
```bash
cd dotnet
dotnet test
```

### Node.js
```bash
cd nodejs
npm test
```

## 📊 Base de Datos

### Esquema Principal

**Tabla `personas`:**
- `id` (PK)
- `nombreCompleto` (VARCHAR 200)
- `identificacion` (VARCHAR 20, UNIQUE)
- `edad` (INT)
- `genero` (VARCHAR 10)
- `estado` (VARCHAR 20)
- `atributosAdicionales` (JSON/NVARCHAR(MAX))
- `fechaCreacion` (DATETIME)
- `fechaModificacion` (DATETIME)

**Tabla `atributo_tipos`:**
- `id` (PK)
- `nombre` (VARCHAR 100)
- `descripcion` (VARCHAR 500)
- `tipoDato` (VARCHAR 20)
- `esObligatorio` (BIT)
- `activo` (BIT)

**Tabla `persona_atributos`:**
- `id` (PK)
- `personaId` (FK)
- `atributoTipoId` (FK)
- `valor` (VARCHAR 500)

## 🔍 Decisiones Técnicas

### 1. Estrategia de Atributos Dinámicos
**Decisión:** Estrategia mixta (JSON + Tabla pivote)
**Justificación:** 
- Flexibilidad para atributos simples (JSON)
- Estructura para validaciones complejas (tabla pivote)
- Balance entre performance y funcionalidad

### 2. Arquitectura en Capas (.NET)
**Decisión:** Clean Architecture con separación clara de responsabilidades
**Justificación:**
- Mantenibilidad y testabilidad
- Separación de concerns
- Facilita escalabilidad

### 3. Repository Pattern
**Decisión:** Implementado en ambas versiones
**Justificación:**
- Abstracción de acceso a datos
- Facilita testing con mocks
- Independencia de la tecnología de persistencia

### 4. Autenticación JWT
**Decisión:** JWT con roles simples (Admin/Consultor)
**Justificación:**
- Stateless y escalable
- Fácil implementación
- Suficiente para el alcance del ejercicio

## 📈 Características Adicionales

- ✅ Logging estructurado
- ✅ Validaciones de entrada
- ✅ Manejo de errores centralizado
- ✅ Rate limiting
- ✅ CORS configurado
- ✅ Health checks
- ✅ Documentación automática (Swagger)
- ✅ Tests unitarios completos
- ✅ Datos de prueba incluidos

## 🚀 Próximos Pasos

Para una implementación en producción, considerar:

1. **Seguridad:**
   - Hash de contraseñas
   - Validación de entrada más robusta
   - Rate limiting por usuario

2. **Performance:**
   - Caching (Redis)
   - Paginación en listados
   - Índices de base de datos optimizados

3. **Monitoreo:**
   - Métricas de aplicación
   - Alertas automáticas
   - Logs centralizados

4. **Escalabilidad:**
   - Microservicios
   - Message queues
   - Load balancing

## 📞 Contacto

Para cualquier consulta sobre la implementación, no dudes en contactarme.

---

**Nota:** Este es un ejercicio técnico de evaluación. Las credenciales y configuraciones mostradas son solo para propósitos de demostración.
