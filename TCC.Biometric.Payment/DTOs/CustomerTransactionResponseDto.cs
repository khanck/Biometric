using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class CustomerTransactionResponseDto
    {
        public Guid Id { get; set; }
        [JsonIgnore]
        public Guid biometricVerification_ID { get; set; }
        [JsonIgnore]
        public Guid paymentCard_ID { get; set; }
        [JsonIgnore]

        public Guid account_ID { get; init; } 
        public TransactionTypes TransactionType { get; set; }

        [StringLength(50)]
        public string? billNumber { get; init; }
        public double amount { get; init; }
        public string createdDate { get; set; }

        [StringLength(50)]
        public string transactionNumber { get; init; }
        public TransactionStatus status { get; set; }

        public BiometricVerificationResponseDto biometricVerification { get; set; }
        public PaymentCardResponseDto paymentCard { get; set; }
        public AccountResponseDto account { get; set; }

        public BusinessResponseDto business { get; set; }
    }
}
