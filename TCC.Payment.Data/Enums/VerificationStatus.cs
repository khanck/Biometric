using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Data.Enums
{
    public enum VerificationStatus
    {
        pending = 1,
        success = 2,
        failed = 3,
        rejected = 4
    }
}
