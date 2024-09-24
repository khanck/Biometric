using System.ComponentModel.DataAnnotations;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class BiometricRequestDto
    {
        public BiometricTypes biometricType { get; init; }
        public string biometricData { get; init; } = null!;      

    }
}
