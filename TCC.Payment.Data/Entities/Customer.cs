using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TCC.Payment.Data.Entities
{
    [Table("Customers")]
    public record Customer : CoreEntity<Guid>
    {
        [StringLength(100)]
        public string firstName { get; init; } = null!;

        [StringLength(100)]
        public string lastName { get; init; } = null!;

        [StringLength(100)]
        public string? email { get; init; } = null!;

        [StringLength(20)]
        public string? mobile { get; init; } = null!;
        [StringLength(100)]
        public string? password  { get; init; } = null!;

        [StringLength(50)]
        public string pin { get; init; } = null!;
        public DateTime createdDate { get; set; }
        public DateTime? updatedDate { get; set; }        
        public CustomerStatus status { get; set; }
        public Boolean? isEmailVerified { get; set; } = false;
        public Boolean? isMobileVerified { get; set; } = false;
        public int TerminalUserId { get; set; }

    }
}
