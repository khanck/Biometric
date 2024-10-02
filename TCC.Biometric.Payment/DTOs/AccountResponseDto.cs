using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class AccountResponseDto
    {
        [JsonIgnore]
        public Guid business_ID { get; set; }
        [StringLength(100)]
        public string bankName { get; init; } = null!;

        [StringLength(50)]
        public string? accountNumber { get; init; }
        [StringLength(50)]
        public string iban { get; init; } = null!;
        public DateTime createdDate { get; set; }

        //public Boolean? isPrimary { get; set; }
        public AccountStatus status { get; set; }
       

    }
}
