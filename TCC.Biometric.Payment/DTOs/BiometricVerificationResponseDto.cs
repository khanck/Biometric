using System.ComponentModel.DataAnnotations;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class BiometricVerificationResponseDto
    {
        public BiometricTypes biometricType { get; init; }
        //[StringLength()]
        public string biometricData { get; init; }
        public DateTime createdDate { get; set; }
        [StringLength(1000)]
        public string verificationResponse { get; set; }
        public VerificationStatus verificationStatus { get; set; }

    }
}
