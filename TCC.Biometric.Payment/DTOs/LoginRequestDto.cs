using System.ComponentModel.DataAnnotations;
using TCC.Payment.Data.Enums;
//using TCC.Payment.Data.Enums;


namespace TCC.Biometric.Payment.DTOs
{
    public class LoginRequestDto
    {    
        [EmailAddress]
        public string email { get; init; } = null!;

        [StringLength(100)]
        public string? password { get; init; } = null!;    
    
    }
}
