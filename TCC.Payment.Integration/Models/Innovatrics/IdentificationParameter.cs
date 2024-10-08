using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models.Innovatrics
{
    public class IdentificationParameter
    {
        public int candidatesCount { get; set; } = 3;
        public int threshold { get; set; } = 85;

        public List<string> galleries { get; set; } = ["default"];
    }
}
