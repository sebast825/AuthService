using Aplication.Services;
using Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services
{
    [TestClass]
    public class LoginAttemptsCacheServiceTest
    {
        private LoginAttemptsCacheServiceI _LoginAttemptsCacheService;
        private  IMemoryCache _cache;

        [TestInitialize]
        public void SetUp()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _LoginAttemptsCacheService = new EmailAttemptsService(_cache);
        }

        [TestMethod]
        public void IsBlocked_EmailAndIpAreBlocked_ReturnTrue()
        {
            string email = "test@gmail.com";
            string ip = "1.0.0.0";
            _cache.Set(email, 5);
            _cache.Set(ip, 30);

           bool isBlocked = _LoginAttemptsCacheService.IsBlocked(email, ip);
            Assert.IsTrue(isBlocked);
        }
        [TestMethod]
        public void IsBlocked_EmailIsBlocked_ReturnTrue()
        {
            string email = "test@gmail.com";
            string ip = "1.0.0.0";
            _cache.Set(email, 4);
            _cache.Set(ip, 30);

            bool isBlocked = _LoginAttemptsCacheService.IsBlocked(email, ip);
            Assert.IsTrue(isBlocked);
        }
        [TestMethod]
        public void IsBlocked_IpIsBlocked_ReturnTrue()
        {
            string email = "test@gmail.com";
            string ip = "1.0.0.0";
            _cache.Set(email, 5);
            _cache.Set(ip, 20);

            bool isBlocked = _LoginAttemptsCacheService.IsBlocked(email, ip);
            Assert.IsTrue(isBlocked);
        }
        [TestMethod]
        public void IsBlocked_EmailAndIpAreAviable_ReturnFalse()
        {
            string email = "test@gmail.com";
            string ip = "1.0.0.0";
            _cache.Set(email, 3);
            _cache.Set(ip, 20);

            bool isBlocked = _LoginAttemptsCacheService.IsBlocked(email, ip);
            Assert.IsFalse(isBlocked);
        }
    }
}
