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

    [Table("Triggers")]
    public record Trigger : CoreEntity<Guid>
    {
        [StringLength(50)]
        public string device_ID { get; set; }
        public Guid account_ID { get; set; }

        [StringLength(50)]
        public string? billNumber { get; set; }
        public double amount { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime? updatedDate { get; set; }
        public TriggerStatus status { get; set; }
    }
}
