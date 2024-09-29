using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Integration.Models
{
    public class DownloadInfo
    {
        public int Total { get; set; }
        public int Offset { get; set; }
    }
    public class SaveUserToTerminalDto
    {
        public SaveUserToTerminalDto()
        {
                DownloadInfo = new DownloadInfo();
        }
        public DownloadInfo DownloadInfo { get; set; }
        public int UserId { get; set; }
        public int TerminalId { get; set; }
    }
}
