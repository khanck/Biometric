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

    [Table("Accounts")]
    public record Account : CoreEntity<Guid>
    {
        [ForeignKey("business")]
        public Guid business_ID { get; set; } = default!;
        [StringLength(100)]
        public string bankName { get; init; } = null!;

        [StringLength(50)]
        public string? accountNumber { get; init; }
        [StringLength(50)]
        public string iban { get; init; } = null!;
        public DateTime createdDate { get; set; }

        //public Boolean? isPrimary { get; set; }
        public AccountStatus status { get; set; }


        public virtual Business business { get; set; }
    }
}
