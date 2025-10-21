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
    public class EmailAttemptsServiceTests
    {
        private IEmailAttemptsService _emailAttemptsService;
        private  IMemoryCache _cache;

        [TestInitialize]
        public void SetUp()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _emailAttemptsService = new EmailAttemptsService(_cache);
        }

        [TestMethod]
        public void IsBlocked_EmailIsBlocked_ReturnTrue()
        {
            string email = "test@gmail.com";
            _cache.Set(email, 5);
  
           bool isBlocked = _emailAttemptsService.EmailIsBlocked(email);
            Assert.IsTrue(isBlocked);
        }
        [TestMethod]
        public void IsBlocked_EmailIsNotBlocked_ReturnTrue()
        {
            string email = "test@gmail.com";
            _cache.Set(email, 4);

            bool isBlocked = _emailAttemptsService.EmailIsBlocked(email);
            Assert.IsFalse(isBlocked);
        }

        [TestMethod]
        public void ResetAttempts_WithExistingAttempts_RevemosFromCache()
        {
            string email = "test@gmail.com";
            int attempts = 4;
            _cache.Set(email, attempts);
            _emailAttemptsService.ResetAttempts(email);

            Assert.AreEqual(_cache.Get(email), null);
        }
        [TestMethod]
        public void IncrementAttempts_WithExistingAttempts_IncreasesCount()
        {
            string email = "test@gmail.com";
            int attempts = 4;
            _cache.Set(email, attempts);
            _emailAttemptsService.IncrementAttempts(email);

            Assert.AreEqual(_cache.Get(email), attempts+1);
        }
        [TestMethod]
        public void IncrementAttempts_WithNotExistingAttempts_IncreasesCount()
        {
            string email = "test@gmail.com";
            int attempts = 1;
            _emailAttemptsService.IncrementAttempts(email);

            Assert.AreEqual(_cache.Get(email), attempts);
        }
    }
}
