using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models.Innovatrics
{
    public class AbisResponse
    {       
        public string externalId { get; set; }
        public Int32 id { get; set; }
        public string gallery { get; set; }
        public int score { get; set; }

        public List<AbisResponse> searchResult { get; set; }

        public bool IsSuccess { get; set; }=false;
        public AbisError error { get; set; }
    }

}
