using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Interfaces;

namespace TCC.Payment.Data.Repositories
{
    public class BiometricVerificationRepository : CoreRepository<BiometricVerification>, IBiometricVerificationRepository
    {
        public BiometricVerificationRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
      

    }
}
