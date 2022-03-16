using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tyre.WSL
{
    public class JwtSettings
    {
        public string JWT_Secret { get; set; }
        public string JWT_Expire { get; set; }
    }
}