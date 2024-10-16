using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class TriggerRequestDto
    {       
        [StringLength(50)]
        public string device_ID { get; set; }
        public Guid account_ID { get; set; }

        [StringLength(50)]
        public string? billNumber { get; set; }
        public double amount { get; set; }      

    }
}
