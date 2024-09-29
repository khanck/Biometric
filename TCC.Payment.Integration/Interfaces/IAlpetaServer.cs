using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Integration.Config;
using TCC.Payment.Integration.Models;

namespace TCC.Payment.Integration.Interfaces
{
    public interface IAlpetaServer
    {
         Task<AlpetaConfiguration> Login();
        Task<BiometricAuthentication> VerifyUserBiometric(string userId);
        Task<BiometricAuthentication> GetCurrentUserBiometric();
        Task<BiometricAuthentication> GetCurrentUserBiometric(string userID);
        Task<BiometricAuthentication> GetVerificationDetails(Int64 index);
       // Task<AlpetaConfiguration> GetAuthentication(string userId);
        Task<AlpetaConfiguration> CreateUser(CreateUserRequestDTO userId);
        Task<AlpetaConfiguration> UpdateUserPicture(UpdateUserPictureReqDTO updateUserPictureReqDTO);
        Task<BiometricVerification> VerifyUserBiometric(string userId);
        Task<BiometricVerification> GetCurrentUserBiometric();
        Task<BiometricVerification> GetVerificationDetails(Int64 index);
        Task<AlpetaConfiguration> SaveUserToTerminal(SaveUserToTerminalDto saveUserToTerminalDto);
    }
}
