using System.ComponentModel.DataAnnotations;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class BusinessRequestDto
    {
        [StringLength(200)]
        public string name { get; init; } = null!;
        public BusinessTypes businessTypes { get; set; }

        [StringLength(100)]
        public string? email { get; init; } = null!;

        [StringLength(20)]
        public string? mobile { get; init; } = null!;
        [StringLength(100)]
        public string? password { get; init; } = null!;

        [StringLength(200)]
        public string? address { get; init; } = null!;

        public  AccountRequestDto account { get; set; }
    }
}
