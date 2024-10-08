using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Config
{
    public class InnovatricsConfiguration
    {
        public string Endpoint { get; set; }
        public int ApiReqTimeout { get; set; }
        public int Threshold { get; set; } = 85;

    }
}
