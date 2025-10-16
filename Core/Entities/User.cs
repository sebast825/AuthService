using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User : ClassBase
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string? FullName { get; set; }

        public ICollection<LoginAttempt> LoginAttempts { get; set; } = new List<LoginAttempt>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
