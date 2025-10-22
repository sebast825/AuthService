# AuthService

```
AuthServiceSolution/
│
├─ Core/
│  ├─ Constants/
│  ├─ Dto/                     
│  ├─ Entities/
│  ├─ Interfaces/
│
├─ Infrastructure/           
│  ├─ Data/
│  ├─ Repositories/ 
│
├─ Application/              
│  ├─ Services/
│  └─ UseCases/               
│
└─ Api/                    
│  ├─ Controllers/
│  ├─ Extensions/
│  ├─ Program.cs
│  ├─ appsettingExample.json
│
└─ Tests/    
```

## Seguridad Implementada

- Autenticación JWT con refresh tokens
- Límite de Peticiones por IP - Limitación global de requests para prevenir DDoS y abuso
- Hashing de contraseñas con BCrypt (work factor 12)
- Registro de intentos de login (exitosos y fallidos)
- Bloqueo temporal tras múltiples intentos fallidos
- Revocación de refresh tokens en logout
- Validación de tokens sin exposición de información sensible

## Auditoría & Monitoreo

- UserLoginHistory: Tracking de logins exitosos para analytics
- SecurityLoginAttempt: Monitorización de intentos fallidos y bloqueos
- EmailAttemptsService: Prevención de fuerza bruta en tiempo real

  
## Caso de uso: Login

- Validación de credenciales con manejo de bloqueos
- Invalidación de tokens anteriores al generar nuevos
- Transacciones para consistencia en persistencia
- Captura de IP y device info para auditoría

## Testing Strategy

- Suite de tests organizada por capas (Entities, Services, UseCases)
- Cobertura de componentes críticos: UserService, RefreshToken, EmailAttempts
- Tests específicos para Auth Use Case (flujo completo login)
- Tests de entidades para validaciones de dominio

## Capa de Datos

- Entity Framework Core con DbContext configurado
- SQL Server como base de datos principal
- Repository pattern para abstracción del data access
- Configuración via connection strings (ambiente-aware)

-------------------------


## Proximas Mejoras

- Persistir los intentos fallidos de login en la base de datos usando un sistema de colas, para evitar saturar el pool de conexiones durante ataques de fuerza bruta
- Implementar middleware y configuración de proxies para capturar con precisión la IP real del cliente, mejorando seguridad, auditoría y bloqueos por IP en entornos de producción
- Endpoint de recuperación de contraseña con tokens seguros
