using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models.Innovatrics
{
    public class Identification
    {
        public string gallery { get; set; } = "default";
        public IdentificationParameter identificationParameters { get; set; }
        public Probe probe { get; set; }
    }
}
