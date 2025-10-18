using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.User
{
    public class UserResponseDto
    {
        public required int Id { get; set; }
        public string FullName { get; set; }
    }
}
