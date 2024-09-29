using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models
{
    public class BiometricAuthentication: AlpetaResult
    {
        public List<BiometricAuthenticationDetail> AuthLogList { get; set; }  // list of Verifications 
        public BiometricAuthenticationDetail AuthLogDetail { get; set; } //Biometric Authentication  Detail


    }
}
