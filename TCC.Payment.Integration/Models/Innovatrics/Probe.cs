using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models.Innovatrics
{
    public class Probe
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string dataBytes { get; set; } //for templet
        public List<AbisImage> faces { get; set; }  //for image 

    }
}
