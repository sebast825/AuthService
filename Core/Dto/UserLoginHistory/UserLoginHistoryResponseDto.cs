using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserLoginHistory
{
    public class UserLoginHistoryResponseDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string IpAddress { get; set; }
        public string DeviceInfo { get; set; } 
    }
}
