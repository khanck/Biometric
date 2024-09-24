using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Integration.Config;

namespace TCC.Payment.Integration.Interfaces
{
    public interface IAlpetaServer
    {
         Task<AlpetaConfiguration> Login();
        Task<AlpetaConfiguration> GetAuthentication(string userId);
    }
}
