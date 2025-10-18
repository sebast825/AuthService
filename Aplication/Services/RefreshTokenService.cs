using Azure.Core.GeoJson;
using Core.Entities;
using Core.Interfaces.Repositories;
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

        private readonly RefreshTokenRepositoryI _refreshTokenRepositoryI;
        public RefreshTokenService(RefreshTokenRepositoryI refreshTokenRepositoryI)
        {
            _refreshTokenRepositoryI = refreshTokenRepositoryI;
        }
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
        public async Task AddAsync(RefreshToken token)
        {
            await _refreshTokenRepositoryI.AddAsync(token);
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
