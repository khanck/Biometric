using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models
{
    public class BiometricVerificationDetail
    {
        public Int64 IndexKey { get; set; }
        public int TerminalID { get; set; }
        public string UserID { get; set; }
        public int GroupCode { get; set; }
        public string UserName { get; set; }
        public string UniqueID { get; set; }
        public DateTime EventTime { get; set; }
        public DateTime ServerRecordTime { get; set; }
        public int AuthType { get; set; }
        public int AuthResult { get; set; }
        public string Card { get; set; }
        public int Func { get; set; }
        public int FuncType { get; set; }
        public int IsPicture { get; set; }
        public string UserImage { get; set; }
        public string LogImage { get; set; }
        public int UserType { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public string Property { get; set; }
        public string TerminalName { get; set; }
        public int ReserveType { get; set; }
        public string ReserveData { get; set; }


    }
}
