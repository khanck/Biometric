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
        Task<AlpetaConfiguration> GetAuthentication(string userId);
        Task<AlpetaConfiguration> CreateUser(CreateUserRequestDTO userId);
        Task<AlpetaConfiguration> UpdateUserPicture(UpdateUserPictureReqDTO updateUserPictureReqDTO);
    }
}
