using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Integration.Models.Innovatrics;

namespace TCC.Payment.Integration.Interfaces
{
    public interface IInnovatricsAbis
    {
        Task<AbisResponse> EnrollPerson(AbisEnrollUser person);
        Task<AbisResponse> IdentifyByFace(Identification request);
        Task<AbisResponse> DeletePerson(string externalId);
    }
}
