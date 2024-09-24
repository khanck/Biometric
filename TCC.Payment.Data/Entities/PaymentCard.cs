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


    [Table("PaymentCards")]
    public record PaymentCard : CoreEntity<Guid>
    {
        [ForeignKey("customer")]
        public Guid customer_ID { get; init; } = default!;

        [StringLength(100)]
        public string nameOnCard { get; init; } = null!;

        [StringLength(20)]
        public string cardType { get; init; } = null!;
        [StringLength(20)]
        public string cardNumber { get; init; } = null!;
        [StringLength(100)]
        public string expiryMonth { get; init; } = null!;
        [StringLength(100)]
        public string expiryYear { get; init; } = null!;
        [StringLength(100)]
        public string cvv { get; init; }   
        public DateTime createdDate { get; set; }
        public DateTime? updatedDate { get; set; }
        public CardStatus status { get; set; }
        public Boolean? isPrimary { get; set; }


        public virtual Customer customer { get; set; }
    }
}
