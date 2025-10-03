# Personas ABM - Implementación .NET

## 🚀 Inicio Rápido

### Prerrequisitos
- .NET 8 SDK
- SQL Server LocalDB (incluido con Visual Studio)
- Visual Studio 2022 o VS Code

### Instalación

1. **Restaurar dependencias:**
```bash
dotnet restore
```

2. **Configurar base de datos:**
```bash
dotnet ef database update --project PersonasABM.Infrastructure --startup-project PersonasABM.API
```

3. **Ejecutar la aplicación:**
```bash
dotnet run --project PersonasABM.API
```

4. **Acceder a Swagger:**
- URL: `http://localhost:5000`
- La documentación interactiva estará disponible automáticamente

## 🏗️ Arquitectura

### Capas del Proyecto

```
PersonasABM.API/              # Capa de Presentación
├── Controllers/              # Controladores REST
├── Program.cs                # Configuración de la aplicación
└── appsettings.json         # Configuración

PersonasABM.Application/      # Capa de Aplicación
├── DTOs/                     # Data Transfer Objects
├── Interfaces/               # Contratos de servicios
└── Services/                 # Lógica de negocio

PersonasABM.Domain/           # Capa de Dominio
├── Entities/                 # Entidades de negocio
└── Enums/                    # Enumeraciones

PersonasABM.Infrastructure/   # Capa de Infraestructura
├── Data/                     # Contexto de EF
├── Repositories/             # Implementación de repositorios
└── DependencyInjection.cs    # Configuración de DI

PersonasABM.Tests/            # Tests Unitarios
└── Services/                 # Tests de servicios
```

## 🔧 Configuración

### Base de Datos
La aplicación usa SQL Server LocalDB por defecto. La cadena de conexión se encuentra en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PersonasABM;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### JWT Configuration
```json
{
  "JwtSettings": {
    "SecretKey": "MiClaveSecretaSuperSeguraParaJWT2024!",
    "Issuer": "PersonasABM",
    "Audience": "PersonasABMUsers",
    "ExpirationHours": 24
  }
}
```

## 🔐 Autenticación

### Usuarios Demo
| Usuario | Contraseña | Rol | Permisos |
|---------|------------|-----|----------|
| admin | admin123 | Admin | CRUD completo |
| consultor | consultor123 | Consultor | Solo lectura |

### Obtener Token
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
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
dotnet test
```

### Tests Incluidos
- ✅ PersonaServiceTests - Tests de lógica de negocio
- ✅ Validaciones de entrada
- ✅ Casos de error
- ✅ Flujos de éxito

## 📊 Modelo de Datos

### Entidades Principales

**Persona:**
```csharp
public class Persona
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; }
    public string Identificacion { get; set; }
    public int Edad { get; set; }
    public string Genero { get; set; }
    public string Estado { get; set; }
    public string AtributosAdicionales { get; set; } // JSON
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
}
```

**AtributoTipo:**
```csharp
public class AtributoTipo
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string? Descripcion { get; set; }
    public string TipoDato { get; set; }
    public bool EsObligatorio { get; set; }
    public bool Activo { get; set; }
}
```

## 🔍 Características Técnicas

### Validaciones
- Data Annotations en entidades
- Validaciones de negocio en servicios
- Validación de identificación única

### Logging
- Microsoft.Extensions.Logging
- Logs estructurados
- Diferentes niveles por ambiente

### Seguridad
- JWT Authentication
- Authorization por roles
- CORS configurado
- Rate limiting

### Performance
- Entity Framework optimizado
- Queries eficientes
- Lazy loading deshabilitado

## 🚀 Despliegue

### Desarrollo
```bash
dotnet run --project PersonasABM.API
```

### Producción
```bash
dotnet publish -c Release -o ./publish
cd publish
dotnet PersonasABM.API.dll
```

### Docker (Opcional)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY ./publish /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "PersonasABM.API.dll"]
```

## 📈 Monitoreo

### Health Check
- Endpoint: `/health`
- Información de estado de la aplicación

### Logs
- Consola en desarrollo
- Archivos en producción
- Niveles configurables

## 🔧 Troubleshooting

### Problemas Comunes

1. **Error de conexión a BD:**
   - Verificar que SQL Server LocalDB esté instalado
   - Ejecutar `dotnet ef database update`

2. **Error de JWT:**
   - Verificar configuración en appsettings.json
   - Usar credenciales correctas

3. **Error de CORS:**
   - Verificar configuración en Program.cs
   - Ajustar políticas según necesidad

## 📞 Soporte

Para problemas técnicos o consultas sobre la implementación, revisar:
1. Logs de la aplicación
2. Documentación de Swagger
3. Tests unitarios como ejemplos de uso
