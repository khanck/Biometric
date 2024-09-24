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

    [Table("Transactions")]
    public record Transaction : CoreEntity<Guid>
    {
        [ForeignKey("biometricVerification")]
        public Guid biometricVerification_ID { get; set; } = default!;
        [ForeignKey("paymentCard")]
        public Guid paymentCard_ID { get; init; } = default!;
        [ForeignKey("account")]
        public Guid account_ID { get; init; } = default!;
        public TransactionTypes TransactionType { get; set; }

        [StringLength(50)]
        public string? billNumber { get; init; }
        public double amount { get; init; }
        public DateTime createdDate { get; set; }

        [StringLength(50)]
        public string transactionNumber { get; set; }
        public TransactionStatus status { get; set; }




        public virtual BiometricVerification biometricVerification { get; set; }
        public virtual PaymentCard paymentCard { get; set; }
        public virtual Account account { get; set; }
    }
}
