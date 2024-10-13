using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models.Innovatrics
{
    public class AbisError
    {
        public string errorCode { get; set; }        
        public string errorMessage { get; set; }
        public string errorDetails { get; set; }
        public string responseUrl { get; set; }

    }

}
