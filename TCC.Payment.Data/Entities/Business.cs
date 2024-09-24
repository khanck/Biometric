using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Enums;

namespace TCC.Payment.Data.Entities
{

    [Table("Business")]
    public record Business : CoreEntity<Guid>
    {       

        [StringLength(200)]
        public string name { get; init; } = null!;
        public BusinessTypes businessTypes { get; set; }

        [StringLength(100)]
        public string? email { get; init; } = null!;

        [StringLength(20)]
        public string? mobile { get; init; } = null!;
        [StringLength(100)]
        public string? password { get; init; } = null!;

        [StringLength(200)]
        public string? address { get; init; } = null!;

        public DateTime createdDate { get; set; }
        public DateTime? updatedDate { get; set; }

        public Boolean? isEmailVerified { get; set; }
        public Boolean? isMobileVerified { get; set; }
        public BusinessStatus status { get; set; }
              


    }
}
