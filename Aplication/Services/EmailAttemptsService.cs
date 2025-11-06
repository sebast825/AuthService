using Core.Interfaces.Services;
using System.Collections.Concurrent;
namespace Aplication.Services
{
    public class EmailAttemptsService : IEmailAttemptsService
    {
        //similar t IMemoryCache but is Thrade Safe
        //if recibe many petitions from the same key will not colapse
        private readonly ConcurrentDictionary<string, (int attempts, DateTime expiry)> _cache;
        private readonly TimeSpan _window = TimeSpan.FromMinutes(5);
        private readonly int _userLimit = 5;

        public EmailAttemptsService()
        {
            _cache = new ConcurrentDictionary<string, (int, DateTime)>();
        }

        public bool EmailIsBlocked(string emailKey)
        {
            CleanExpired();
            var entry = _cache.GetOrAdd(emailKey,
                k => (0, DateTime.UtcNow.Add(_window)));

            return entry.attempts >= _userLimit;
        }

        public void IncrementAttempts(string key)
        {
            CleanExpired();
            _cache.AddOrUpdate(key,
                (1, DateTime.UtcNow.Add(_window)),
                (k, v) =>
                {
                    var newAttempts = v.attempts + 1;
                    return (newAttempts, v.expiry);
                });
        }

        public void ResetAttempts(string key)
        {
            _cache.TryRemove(key, out _);
        }

        private void CleanExpired()
        {
            var now = DateTime.UtcNow;
            foreach (var item in _cache.Where(x => x.Value.expiry <= now).ToList())
            {
                _cache.TryRemove(item.Key, out _);
            }
        }
    }
}