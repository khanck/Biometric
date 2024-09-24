using System.Net.Http.Headers;

namespace TCC.DigitalID.Services.Config
{
    public interface IAuthenticationService
    {
        bool IsValidUser(AuthenticationHeaderValue authHeader);
        bool IsValidApiKey(AuthenticationHeaderValue authHeader);
        bool IsValidNafathApiKey(AuthenticationHeaderValue authHeader);
    }
}
