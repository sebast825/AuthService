using Azure.Core.GeoJson;
using Core.Constants;
using Core.Dto.RefreshToken;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {

        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        public RefreshToken CreateRefreshToken(int userId)
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
            await _refreshTokenRepository.AddAsync(token);
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {
            RefreshToken? refreshToken = await _refreshTokenRepository.GetAsync(t => t.Token == token);
            if (refreshToken == null) {
                throw new InvalidCredentialException(ErrorMessages.InvalidToken);
            }
            refreshToken.Revoked = true;
            await _refreshTokenRepository.UpdateAsync(refreshToken);
        }
        public async Task RevokeRefreshTokenIfExistAsync(int userId)
        {
            RefreshToken? refreshToken = await _refreshTokenRepository.GetAsync(t => t.User.Id == userId);
            if (refreshToken != null)
            {
                refreshToken.Revoked = true;
                await _refreshTokenRepository.UpdateAsync(refreshToken);
            }
         
        }
        public async Task<RefreshTokenResponseDto> GetValidRefreshTokenAsync(string refreshToken)
        {
            RefreshToken? refreshTokenEntity = await _refreshTokenRepository.GetAsync(t => t.Token == refreshToken);
            if (refreshTokenEntity == null)
            {
                throw new InvalidCredentialException(ErrorMessages.InvalidToken);
            }else if (!refreshTokenEntity.IsActive())
            {
                throw new InvalidCredentialException();
            }
            
                return new RefreshTokenResponseDto
                {
                    UserId = refreshTokenEntity.UserId,
                    ExpiresAt = refreshTokenEntity.ExpiresAt,
                    Revoked = refreshTokenEntity.Revoked,
                    Token = refreshTokenEntity.Token

                };
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
