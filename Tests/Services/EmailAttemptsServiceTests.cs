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

        [TestInitialize]
        public void SetUp()
        {
            _emailAttemptsService = new EmailAttemptsService();
        }

        private void AddAttempts(int amount, string email)
        {

            int i = 0;
            while (i < amount)
            {
                _emailAttemptsService.IncrementAttempts(email);
                i++;
            }

        }
        [TestMethod]
        public void IsBlocked_EmailIsBlocked_ReturnTrue()
        {
            string email = "test@gmail.com";
            AddAttempts(5, email);

            bool isBlocked = _emailAttemptsService.EmailIsBlocked(email);
            Assert.IsTrue(isBlocked);
        }
        [TestMethod]
        public void IsBlocked_EmailIsNotBlocked_ReturnTrue()
        {
            string email = "test@gmail.com";
            AddAttempts(4, email);

            bool isBlocked = _emailAttemptsService.EmailIsBlocked(email);
            Assert.IsFalse(isBlocked);
        }

        [TestMethod]
        public void ResetAttempts_WithExistingAttempts_RevemosFromCache()
        {
            string email = "test@gmail.com";
            int attempts = 6;
            AddAttempts(attempts, email);
            _emailAttemptsService.ResetAttempts(email);
            bool isBlocked = _emailAttemptsService.EmailIsBlocked(email);

            Assert.IsFalse(isBlocked);
        }


    }
}
