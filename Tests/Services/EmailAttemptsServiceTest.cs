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
    public class EmailAttemptsServiceTest
    {
        private EmailAttemptsServiceI _EmailAttemptsServiceI;
        private  IMemoryCache _cache;

        [TestInitialize]
        public void SetUp()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _EmailAttemptsServiceI = new EmailAttemptsService(_cache);
        }

        [TestMethod]
        public void IsBlocked_EmailIsBlocked_ReturnTrue()
        {
            string email = "test@gmail.com";
            _cache.Set(email, 5);
  
           bool isBlocked = _EmailAttemptsServiceI.EmailIsBlocked(email);
            Assert.IsTrue(isBlocked);
        }
        [TestMethod]
        public void IsBlocked_EmailIsNotBlocked_ReturnTrue()
        {
            string email = "test@gmail.com";
            _cache.Set(email, 4);

            bool isBlocked = _EmailAttemptsServiceI.EmailIsBlocked(email);
            Assert.IsFalse(isBlocked);
        }
        
    }
}
