using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models
{
    public class UpdateUserPictureReqDTO
    {
        public string ImageType { get; set; }
        public string Picture { get; set; }
        public int UserId { get; set; }
    }
}
