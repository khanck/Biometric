using System.ComponentModel.DataAnnotations;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public record CustomerResponseDto
    {
        public string Id { get; set; }
        public string firstName { get; init; } 

        public string lastName { get; init; }
    
        public string email { get; init; } 

        public string mobile { get; init; } 
       
        //public string? password { get; init; }

        //public string pin { get; init; } 

        public CustomerStatus status { get; set; }


        public List<BiometricResponseDto> biometric { get; set; }

    }
}
