using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public static class ErrorMessages
    {
        public const string PasswordLengthMin = "La contraseña debe tener al menos 8 caracteres";
        public const string EmailFormat = "El formato del email es inválido";
        public const string EmailNotAviable = "No es posible usar este email. Por favor, pruebe otro";
        public const string InvalidCredentials = "Las credenciales son invalidas";
        public const string InvalidToken = "El token es invalido";
        public const string MaxLoginAttemptsExceeded = "Demasiados intentos fallidos. Espere 5 minutos antes de intentar nuevamente.";

    }
}
