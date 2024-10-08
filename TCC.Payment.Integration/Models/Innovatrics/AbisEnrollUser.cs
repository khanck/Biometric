using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models.Innovatrics
{
    public class AbisEnrollUser
    {
        public string externalId { get; set; }
        public DateTime enrolledAt { get; set; }
        public EnrollAction enrollAction { get; set; }
        public PersonInfo customDetails { get; set; }
        public Modality faceModality { get; set; }

    }

}
