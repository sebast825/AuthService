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
  
## Caso de uso: Login

**1. Recepción de credenciales:**  
- Se reciben email y password, junto con IP y datos del dispositivo.

**2. Validación de bloqueos:**  
- Se verifica si el email está bloqueado por intentos fallidos anteriores.  
- Si está bloqueado, se registra el intento y se rechaza el login.

**3. Validación de credenciales:**  
- Si las credenciales son inválidas, se registra el fallo y se notifica al usuario.  
- Si son válidas, se continúa con el login.

**4. Manejo de login exitoso:**  
- Se resetean los intentos de email.  
- Se registra el intento exitoso en el historial de login.
- Se invalida el Refresh Token existente.
- Se generan JWT y Refresh Token.  
- Se persiste el Refresh Token en la base de datos dentro de una transacción.

**5. Respuesta:**  
- Se retorna un objeto con `AccessToken`, `RefreshToken` y datos del usuario.



-------------------------

### UserLoginHistory
- Guarda logins exitosos de usuarios registrados
- Para analytics, historial de actividad, reporting

### SecurityLoginAttempt 
- Guarda intentos fallidos y bloqueos
- Para detección de ataques, forensics, monitoreo


### EmailAttemptsService
- Prevención en Tiempo Real
- Maneja bloqueo temporal después de intentos fallidos

## Proximas Mejoras

- Persistir los intentos fallidos de login en la base de datos usando un sistema de colas, para evitar saturar el pool de conexiones durante ataques de fuerza bruta.
- Implementar middleware y configuración de proxies para capturar con precisión la IP real del cliente, mejorando seguridad, auditoría y bloqueos por IP en entornos de producción.
