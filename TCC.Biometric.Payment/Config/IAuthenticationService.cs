using System.Net.Http.Headers;

namespace TCC.Biometric.Payment.Config
{
    public interface IAuthenticationService
    {
        bool IsValidUser(AuthenticationHeaderValue authHeader);
        bool IsValidApiKey(AuthenticationHeaderValue authHeader);        
    }
}
