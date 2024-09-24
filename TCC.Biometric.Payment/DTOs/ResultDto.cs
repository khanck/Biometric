using System.Text.Json.Serialization;

namespace TCC.Biometric.Payment.DTOs
{
    public class ResultDto<T> where T : class
    {
        [JsonPropertyName("error")]
        public ErrorDto? error { get; set; }

        [JsonPropertyName("data")]
        public T? data { get; set; }
        public bool success { get; set; } = false;

    }
}

