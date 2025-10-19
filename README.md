# AuthService

```
AuthServiceSolution/
│
├─ Core/                       
│  ├─ Entities/
│  ├─ Interfaces/
│  ├─ Constants/
│
├─ Infrastructure/           
│  ├─ Data/
│  │   └─ DataContext.cs
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
  
## Caso de uso: LoginUser

1. Traer usuario desde UserRepository por email
   - Si el usuario no existe → lanzar AuthenticationException (usuario inválido)

2. Verificar si puede intentar login
   - LoginAttemptService.CanAttemptLogin(user.LoginAttempts)
   - Si no puede (bloqueado, demasiados intentos fallidos, etc.) → lanzar TooManyAttemptsException

3. Validar credenciales
   - AuthService.ValidateCredentials(user, password)
   - Si es incorrecto:
       • Registrar intento fallido → LoginAttemptService.RecordAttempt(user.Id, success = false)
       • Lanzar AuthenticationException (credenciales inválidas)

4. Registrar intento exitoso
   - LoginAttemptService.RecordAttempt(user.Id, success = true)

5. Generar token
   - TokenService.GenerateAccessToken(user)
   - TokenService.GenerateRefreshToken(user)

6. Actualizar o guardar refresh token en DB si aplica

7. Devolver respuesta
   - Crear y retornar LoginResponseDto con:
       • AccessToken
       • RefreshToken
       • Expiration
       • Datos mínimos del usuario (opcional)

## Proximas Mejoras

- Persistir los intentos fallidos de login en la base de datos usando un sistema de colas, para evitar saturar el pool de conexiones durante ataques de fuerza bruta.
- Implementar middleware y configuración de proxies para capturar con precisión la IP real del cliente, mejorando seguridad, auditoría y bloqueos por IP en entornos de producción.
