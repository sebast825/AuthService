# AuthService

```
AuthServiceSolution/
│
├─ Core/                       
│  ├─ Entities/
│  │   ├─ User.cs
│  │   ├─ LoginAttempt.cs
│  │   └─ RefreshToken.cs
│  ├─ Interfaces/
│     ├─ UserRepositoryI.cs
│     ├─ LoginAttemptRepositoryI.cs
│     └─ AuthServiceI.cs
│
├─ Infrastructure/           
│  ├─ Data/
│  │   └─ DataContext.cs
│  ├─ Repositories/
│     ├─ UserRepository.cs
│     ├─ LoginAttemptRepository.cs
│     └─ RefreshTokenRepository.cs     
│
├─ Application/              
│  ├─ Services/
│  │   └─ AuthService.cs
│  └─ UseCases/               
│      ├─ LoginUser.cs
│      ├─ RefreshTokenUseCase.cs
│      └─ RecordLoginAttempt.cs
│
└─ Api/                    
   ├─ Controllers/
   │   └─ AuthController.cs
   ├─ Program.cs
   ├─ appsettingExample.json
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
