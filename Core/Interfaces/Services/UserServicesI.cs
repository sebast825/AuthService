using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface UserServicesI
    {
        Task AddAsync(string email, string password, string? fullname = null);

    }
}
