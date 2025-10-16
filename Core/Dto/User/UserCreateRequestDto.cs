using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.User
{
    public class UserCreateRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string? FullName { get; set; }
    }
}
