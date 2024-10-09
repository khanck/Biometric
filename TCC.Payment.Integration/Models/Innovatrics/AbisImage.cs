using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models.Innovatrics
{
    public class AbisImage
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string captureDevice { get; set; }
        public string dataBytes { get; set; }
       
    }

}
