using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models
{
    public class BiometricVerification: AlpetaResult
    {
        public List<BiometricVerificationDetail> AuthLogList { get; set; }  // list of Verifications 
        public BiometricVerificationDetail AuthLogDetail { get; set; } //Biometric Verification Detail


    }
}
