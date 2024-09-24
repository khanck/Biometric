using System.ComponentModel.DataAnnotations;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class BiometricVerificationRequestDto
    {
        public BiometricTypes biometricType { get; init; }
        //[StringLength()]
        public string biometricData { get; init; }

    }
}
