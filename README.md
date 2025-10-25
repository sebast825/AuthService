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

## Security Implementation

- JWT Authentication with refresh tokens
- IP Rate Limiting - Global request throttling to prevent DDoS and abuse
- Password hashing with BCrypt (work factor 12)
- Login attempt logging (successful and failed)
- Temporary account lockout after multiple failed attempts
- Refresh token revocation on logout
- Secure token validation without sensitive information exposure

## Audit & Monitoring

- UserLoginHistory: Successful login tracking for analytics and reporting
- SecurityLoginAttempt: Failed attempt monitoring and block tracking
- EmailAttemptsService: Real-time brute force prevention

## Login Use Case

- Credential validation with account lockout handling
- Automatic invalidation of previous tokens when generating new ones
- Transactional persistence for data consistency
- IP address and device information capture for audit trails

## Testing Strategy

- Multi-layer test suite (Entities, Services, UseCases)
- Critical component coverage: UserService, RefreshToken, EmailAttempts
- Comprehensive Auth Use Case tests (full login flow)
- Entity validation tests for domain logic

## Data Layer

- Entity Framework Core with configured DbContext
- SQL Server as primary database
- Repository pattern for data access abstraction
- Environment-aware configuration via connection strings

## Future Improvements

- Implement queue-based persistence for failed login attempts to prevent database connection pool saturation during brute force attacks
- Add proxy configuration middleware to accurately capture real client IP addresses, enhancing security, audit trails, and IP-based blocking in production environments
- Secure password recovery endpoint with time-limited tokens
-  Separation of Responsibilities in AuthUseCase


> In this project, the authentication use case (`AuthUseCase`) is designed with a clear separation of responsibilities in mind. By using coordinators, each aspect of the authentication workflow—such as user validation, token management, and login auditing can be encapsulated separately, making the code more organized, maintainable, and easier to understand. 


| Coordinator       | Responsibility                                      | Internal Services                                       |
|------------------|----------------------------------------------------|--------------------------------------------------------|
| UserCoordinator   | Handle user validation, credentials, and account blocking | `_userServices`, `_emailAttemptsService`             |
| TokenCoordinator  | Generate, revoke, and manage tokens               | `_jwtService`, `_refreshTokenService`                |
| AuditCoordinator  | Record login audits and failed login attempts     | `_loginAttemptsService`, `_securityLoginAttemptService`, `_logger` |


