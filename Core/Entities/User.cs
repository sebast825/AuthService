using Core.Constants;
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

        public ICollection<UserLoginHistory> LoginAttempts { get; set; } = new List<UserLoginHistory>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();


        public void Validate()
        {

            ValidteEmail();
            ValidatePassword();
        }

        private void ValidatePassword()
        {
            if (Password?.Length < 8)
                throw new FormatException(ErrorMessages.PasswordLengthMin);
        }

        private void ValidteEmail()
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(Email);
                
            }
            catch
            {
                throw new FormatException(ErrorMessages.EmailFormat);
            }
        }
    }
}
