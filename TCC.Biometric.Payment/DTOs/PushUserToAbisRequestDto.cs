namespace TCC.Biometric.Payment.DTOs
{
    public class PushUserToAbisRequestDto
    {
        public Guid customer_ID { get; init; } = default!;

        public string enrollActionType { get; set; } 
    }
}
