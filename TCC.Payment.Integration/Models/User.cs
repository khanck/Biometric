using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models
{
    public class User
    {
        public int UserType { get; set; } = 2;
        public string UserId { get; set; } = "Master";
        public string Password { get; set; } = "0000";
    }
}
