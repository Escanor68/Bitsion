# Personas ABM - Implementación Node.js

## 🚀 Inicio Rápido

### Prerrequisitos

- Node.js 18+
- npm o yarn
- SQLite (incluido automáticamente)

### Instalación

1. **Instalar dependencias:**

```bash
npm install
```

2. **Configurar base de datos SQLite:**

```bash
npm run db:setup
```

3. **Compilar TypeScript:**

```bash
npm run build
```

4. **Ejecutar la aplicación:**

```bash
# Desarrollo
npm run dev

# Producción
npm start
```

6. **Acceder a la API:**

- URL: `http://localhost:3000`
- Health Check: `http://localhost:3000/health`

## 🏗️ Arquitectura

### Estructura del Proyecto

```
src/
├── entities/                 # Entidades de dominio
│   ├── Persona.ts
│   ├── AtributoTipo.ts
│   └── PersonaAtributo.ts
├── dto/                     # Data Transfer Objects
│   ├── PersonaDto.ts
│   └── LoginDto.ts
├── repositories/            # Capa de acceso a datos
│   ├── IPersonaRepository.ts
│   └── PersonaRepository.ts
├── services/                # Lógica de negocio
│   ├── IPersonaService.ts
│   └── PersonaService.ts
├── controllers/             # Controladores REST
│   ├── AuthController.ts
│   └── PersonasController.ts
├── middleware/              # Middleware personalizado
│   ├── auth.ts
│   └── validation.ts
├── routes/                  # Definición de rutas
│   ├── auth.ts
│   └── personas.ts
├── config/                  # Configuración
│   └── database.ts
└── server.ts                # Punto de entrada
```

## 🔧 Configuración

### Variables de Entorno (.env)

```env
# Database Configuration (SQLite)
DB_TYPE=sqlite
DB_DATABASE=./PersonasABM.db

# JWT Configuration
JWT_SECRET=MiClaveSecretaSuperSeguraParaJWT2024!
JWT_EXPIRES_IN=24h
JWT_ISSUER=PersonasABM
JWT_AUDIENCE=PersonasABMUsers

# Server Configuration
PORT=3000
NODE_ENV=development
LOG_LEVEL=info
```

### Base de Datos SQLite

La aplicación usa TypeORM con SQLite. La base de datos se crea automáticamente en `./PersonasABM.db` cuando se inicia el servidor.

**Ventajas de SQLite:**

- ✅ Sin configuración de servidor
- ✅ Archivo único y portable
- ✅ Rápido para desarrollo
- ✅ No requiere instalación adicional

## 🔐 Autenticación

### Usuarios Demo

| Usuario   | Contraseña   | Rol       | Permisos      |
| --------- | ------------ | --------- | ------------- |
| admin     | admin123     | Admin     | CRUD completo |
| consultor | consultor123 | Consultor | Solo lectura  |

### Obtener Token

```bash
curl -X POST "http://localhost:3000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "admin123"}'
```

## 📚 Endpoints

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

## 🧪 Testing

### Ejecutar Tests

```bash
# Todos los tests
npm test

# Modo watch
npm run test:watch

# Con coverage
npm run test:coverage
```

### Tests Incluidos

- ✅ PersonaServiceTests - Tests de lógica de negocio
- ✅ PersonasControllerTests - Tests de controladores
- ✅ Validaciones de entrada
- ✅ Casos de error
- ✅ Flujos de éxito

## 📊 Modelo de Datos

### Entidades Principales

**Persona:**

```typescript
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
}
```

**AtributoTipo:**

```typescript
@Entity('atributo_tipos')
export class AtributoTipo {
  @PrimaryGeneratedColumn()
  id: number;

  @Column({ type: 'varchar', length: 100 })
  nombre: string;

  @Column({ type: 'varchar', length: 500, nullable: true })
  descripcion?: string;

  @Column({ type: 'varchar', length: 20, default: 'Texto' })
  tipoDato: string;

  @Column({ type: 'bit', default: false })
  esObligatorio: boolean;
}
```

## 🔍 Características Técnicas

### Middleware

- **Helmet**: Seguridad HTTP
- **CORS**: Cross-Origin Resource Sharing
- **Morgan**: Logging de requests
- **Compression**: Compresión de respuestas
- **Rate Limiting**: Límite de requests por IP

### Validaciones

- **class-validator**: Validaciones de DTOs
- **class-transformer**: Transformación de datos
- Validaciones de negocio en servicios

### Logging

- Morgan para HTTP requests
- Console logging personalizado
- Diferentes niveles por ambiente

### Seguridad

- JWT Authentication
- Authorization por roles
- Helmet para headers de seguridad
- Rate limiting

## 🚀 Scripts Disponibles

```bash
# Desarrollo
npm run dev          # Ejecutar en modo desarrollo con hot reload

# Producción
npm run build        # Compilar TypeScript
npm start           # Ejecutar versión compilada

# Testing
npm test            # Ejecutar tests
npm run test:watch  # Tests en modo watch
npm run test:coverage # Tests con coverage

# Base de datos
npm run db:setup          # Configurar y verificar SQLite
npm run db:check          # Verificar tablas de la base de datos
npm run migration:generate  # Generar migración
npm run migration:run      # Ejecutar migraciones
npm run migration:revert   # Revertir última migración
```

## 📈 Monitoreo

### Health Check

- Endpoint: `GET /health`
- Información de estado de la aplicación
- Uptime y timestamp

### Logs

- Morgan para HTTP requests
- Console logging estructurado
- Diferentes niveles según ambiente

## 🔧 Troubleshooting

### Problemas Comunes

1. **Error de conexión a BD:**

   ```bash
   # Verificar configuración en .env
   # Verificar que SQL Server esté ejecutándose
   npm run migration:run
   ```

2. **Error de TypeScript:**

   ```bash
   # Limpiar y recompilar
   rm -rf dist/
   npm run build
   ```

3. **Error de dependencias:**

   ```bash
   # Limpiar node_modules y reinstalar
   rm -rf node_modules package-lock.json
   npm install
   ```

4. **Error de JWT:**
   - Verificar JWT_SECRET en .env
   - Usar credenciales correctas

## 🐳 Docker (Opcional)

### Dockerfile

```dockerfile
FROM node:18-alpine

WORKDIR /app

COPY package*.json ./
RUN npm ci --only=production

COPY . .
RUN npm run build

EXPOSE 3000

CMD ["npm", "start"]
```

### docker-compose.yml

```yaml
version: '3.8'
services:
  api:
    build: .
    ports:
      - '3000:3000'
    environment:
      - NODE_ENV=production
      - DB_HOST=db
    depends_on:
      - db

  db:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123
    ports:
      - '1433:1433'
```

## 📞 Soporte

Para problemas técnicos o consultas sobre la implementación:

1. **Revisar logs** de la aplicación
2. **Verificar configuración** en .env
3. **Ejecutar tests** para validar funcionalidad
4. **Consultar documentación** de TypeORM y Express

## 🔄 Migraciones

### Generar Migración

```bash
npm run migration:generate -- -n NombreMigracion
```

### Ejecutar Migraciones

```bash
npm run migration:run
```

### Revertir Migración

```bash
npm run migration:revert
```

## 📊 Performance

### Optimizaciones Incluidas

- ✅ Compresión de respuestas
- ✅ Rate limiting
- ✅ Queries optimizadas con TypeORM
- ✅ Lazy loading deshabilitado
- ✅ Índices de base de datos
