using Core.Dto.User;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface RefreshTokenServiceI
    {
        RefreshToken Create(int userId);
        Task AddAsync(RefreshToken token);
        Task RevokeRefreshToken(string token);
    }
}
