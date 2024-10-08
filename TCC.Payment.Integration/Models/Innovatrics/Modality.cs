using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models.Innovatrics
{
    public class Modality
    {
        public List<Face> faces { get; set; }
    }
    public class Face
    {
        public AbisImage image { get; set; }
    }
}
