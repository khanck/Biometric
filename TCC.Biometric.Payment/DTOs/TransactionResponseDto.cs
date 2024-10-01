using System.ComponentModel.DataAnnotations;

using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class TransactionResponseDto
    {
        public Guid Id { get; set; }
        public TransactionTypes TransactionType { get; set; }

        [StringLength(50)]
        public string? billNumber { get; init; }
        public double amount { get; init; }
        public DateTime createdDate { get; set; }

        [StringLength(50)]
        public string transactionNumber { get; init; }
        public TransactionStatus status { get; set; }

        public BiometricVerificationResponseDto biometricVerification { get; set; }
        public PaymentCardResponseDto paymentCard { get; set; }
        public AccountResponseDto account { get; set; }

        public  CustomerResponseDto customer { get; set; }
    }
}
