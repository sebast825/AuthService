using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface JwtServiceI
    {
        public string GenerateAccessToken(string id);
    }
}
