using System.ComponentModel.DataAnnotations;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class AccountRequestDto
    {

        [StringLength(100)]
        public string bankName { get; init; } = null!;

        [StringLength(50)]
        public string? accountNumber { get; init; }
        [StringLength(50)]
        public string iban { get; init; } = null!;

        //public Boolean? isPrimary { get; set; }
     
    }
}
