using Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class LoginAttemptsCacheService : LoginAttemptsCacheServiceI
    {
        private readonly IMemoryCache _cache;
        //range time until the cache restart
        private readonly TimeSpan _window = TimeSpan.FromMinutes(5);
        private readonly int _userLimit = 5;
        private readonly int _ipLimit = 30;
        public LoginAttemptsCacheService(IMemoryCache cache) {
        _cache = cache;
        }


        public bool IsBlocked(string email, string ip)
        {

            return EmailIsBlocked(email) || IpIsBlocked(ip);

        }

        private bool EmailIsBlocked(string emailKey)
        {
            var emailAttempts = this.GetOrCreateLimit(emailKey);
            return emailAttempts >= _userLimit;
        }
        private bool IpIsBlocked(string ipKey)
        {
            var ipAttempts = this.GetOrCreateLimit(ipKey);
            return ipAttempts >= _ipLimit;
        }
        private int GetOrCreateLimit(string key)
        {
            int attempts = _cache.GetOrCreate(key, e =>
            {
                e.AbsoluteExpirationRelativeToNow = _window;
                return 0;
            });
            return attempts; 
        }
    }
}
