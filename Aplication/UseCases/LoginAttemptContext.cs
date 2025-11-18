using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    internal record LoginAttemptContext(string Email, string IpAddress,string DeviceInfo);
       
}
