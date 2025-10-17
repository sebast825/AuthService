using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User : ClassBase
    {

    

        public required string Email { get; set; }
        public required string Password { get; set; }

        public string? FullName { get; set; }

        public ICollection<LoginAttempt> LoginAttempts { get; set; } = new List<LoginAttempt>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();


        public void Validate()
        {

            ValidteEmail();
            ValidatePassword();
        }
        private void ValidatePassword()
        {
            if (Password?.Length < 8)
                throw new FormatException("La contraseña debe tener al menos 8 caracteres");
        }

        private void ValidteEmail()
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(Email);
                
            }
            catch
            {
                throw new FormatException("Formato de email inválido");
            }
        }
    }
}
