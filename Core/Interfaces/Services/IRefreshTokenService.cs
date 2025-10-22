using Core.Dto.RefreshToken;
using Core.Dto.User;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IRefreshTokenService
    {
        RefreshToken CreateRefreshToken(int userId);
        Task AddAsync(RefreshToken token);
        Task RevokeRefreshTokenAsync(string token);
        Task RevokeRefreshTokenIfExistAsync(int userId);
        Task<RefreshTokenResponseDto> GetValidRefreshTokenAsync(string token);
    }
}
