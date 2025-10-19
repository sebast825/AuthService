using Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class EmailAttemptsService : EmailAttemptsServiceI
    {
        private readonly IMemoryCache _cache;
        //range time until the cache restart
        private readonly TimeSpan _window = TimeSpan.FromMinutes(5);
        private readonly int _userLimit = 5;
     
        public EmailAttemptsService(IMemoryCache cache) {
        _cache = cache;
        }

        public bool EmailIsBlocked(string emailKey)
        {
            var emailAttempts = this.GetOrCreateAttempts(emailKey);
            return emailAttempts >= _userLimit;
        }

        public void IncrementFailedAttempts(string key)
        {
            var attempts = GetOrCreateAttempts(key);
            _cache.Set(key, attempts + 1, _window); // Reset counter after _window 
        }
        public void ResetAttempts(string key)
        {
            _cache.Remove(key);
        }

        private int GetOrCreateAttempts(string key)
        {
            int attempts = _cache.GetOrCreate<int>(key, e =>
            {
                e.AbsoluteExpirationRelativeToNow = _window;
                return 0;
            });
            return attempts;
        }
    }
}
