using System.ComponentModel.DataAnnotations;
using TCC.Payment.Data.Enums;
//using TCC.Payment.Data.Enums;


namespace TCC.Biometric.Payment.DTOs
{
    public class CustomerRequestDto
    {

        [StringLength(100)]
        public string firstName { get; init; } = null!;

        [StringLength(100)]
        public string lastName { get; init; } = null!;

        [EmailAddress]
        public string email { get; init; } = null!;

        //[RegularExpression(@"^[0]{1}[0-9]{9}$", ErrorMessage = "invalid mobile number")]
        public string mobile { get; init; } = null!;

        [StringLength(100)]
        public string? password { get; init; } = null!;

        [StringLength(50)]
        public string pin { get; init; } = null!;
 
        public List<BiometricRequestDto> biometric { get; set; }
    

    }
}
