using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Config
{
    public class AlpetaConfiguration
    {
        public string Endpoint { get; set; }
        public int UserType { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public int ApiReqTimeout { get; set; }
    }
}
