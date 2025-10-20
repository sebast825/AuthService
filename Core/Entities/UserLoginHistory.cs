﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UserLoginHistory : ClassBase
    {
        public int UserId { get; set; }
        public string IpAddress { get; set; } = null!;
        public User User { get; set; } = null!;
        public string DeviceInfo { get; set; } = null!;
    }
}
