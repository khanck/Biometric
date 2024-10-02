using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.DTOs
{
    public class PushUserToTerminalRequestDto
    {
        public Guid customer_ID { get; init; } = default!;

        public int TerminalId { get; set; } = 1;


    }
}
