using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Enums;

namespace TCC.Payment.Data.Entities
{
    [Table("BiometricVerifications")]
    public record BiometricVerification : CoreEntity<Guid>
    {
        [ForeignKey("customer")]
        public Guid? customer_ID { get; set; } = default!;
        public BiometricTypes biometricType { get; init; }
        //[StringLength()]
        public string biometricData { get; init; }
        public DateTime createdDate { get; set; }
        [StringLength(1000)]
        public string verificationResponse { get; set; }

        public string verificationID { get; set; }
        public VerificationStatus verificationStatus { get; set; }


        public virtual Customer customer { get; set; }
    }
}
