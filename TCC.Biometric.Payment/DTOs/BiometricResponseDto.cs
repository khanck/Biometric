using System.ComponentModel.DataAnnotations;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class BiometricResponseDto
    {
        public BiometricTypes biometricType { get; init; }

        [StringLength(20)]
        public string abisReferenceID { get; init; }
        public string biometricData { get; init; } = null!;
        public BiometricStatus status { get; set; }

    }
}
