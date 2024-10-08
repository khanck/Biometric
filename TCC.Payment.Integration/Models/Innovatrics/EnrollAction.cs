using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models.Innovatrics
{
    public class EnrollAction
    {
        public string enrollActionType { get; set; } = "None";
        public string referenceExternalId { get; set; }
    }
}
