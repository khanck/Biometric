using System.ComponentModel.DataAnnotations;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class PaymentCardResponseDto
    {
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


    }
}
