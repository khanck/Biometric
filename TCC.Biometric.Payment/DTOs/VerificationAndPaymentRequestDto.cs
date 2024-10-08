using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class VerificationAndPaymentRequestDto
    {
        public Guid account_ID { get; init; }

        [StringLength(50)]
        public string? billNumber { get; init; }
        public double amount { get; init; }

        public BiometricVerificationRequestDto biometric { get; init; }
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "The PIN must be 4 digits long.")]
        public int pin { get; init; }
    }
}
