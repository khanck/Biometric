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
    [Table("Biometrics")]
    public record Biometrics : CoreEntity<Guid>
    {
        [ForeignKey("customer")]
        public Guid customer_ID { get; init; } = default!;     
        public BiometricTypes biometricType { get; init; }

        [StringLength(20)]
        public string abisReferenceID { get; set; }    
        public string biometricData { get; init; } = null!;
        public DateTime createdDate { get; set; } 
        public BiometricStatus status { get; set; }


        public virtual Customer customer { get; set; }
    }
}
