using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.SecurityLoginAttempt
{
    public class SecurityLoginAttemptResponseDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Email { get; set; } = null!;
        public string FailureReason { get; set; } = null!;
        public string IpAddress { get; set; } = null!;
        public string DeviceInfo { get; set; } = null!;
    }
}
