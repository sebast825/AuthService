using Azure.Core.GeoJson;
using Core.Entities;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class RefreshTokenService : RefreshTokenServiceI
    {
        public RefreshToken Create(int userId)
        {
           

            RefreshToken refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = GenerateRefreshToken(),
                ExpiresAt = GenerateExpirationDate()
            };
            return refreshToken;
        
        }


        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private DateTime GenerateExpirationDate()
        {
            return DateTime.UtcNow.AddDays(1);
        }
    }
}
