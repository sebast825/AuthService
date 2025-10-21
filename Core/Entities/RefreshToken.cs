using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class RefreshToken : ClassBase
    {
        public int UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool Revoked { get; set; } = false;

        public User User { get; set; } = null!;

        public bool IsActive()
        {
            return !Revoked && ExpiresAt > DateTime.UtcNow;
        }

    }
}
