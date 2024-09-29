using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class UserPaymentRequestDto
    {
        public Guid customer_ID { get; init; } 
        public Guid account_ID { get; init; }

        [StringLength(50)]
        public string? billNumber { get; init; }
        public double amount { get; init; }
              
    }
}
