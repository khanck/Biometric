using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Payment.Data.Entities
{
    public abstract record CoreEntity<IdType>
    {
        [Key]
        [Required]
        public IdType Id { get; init; } = default!;
    }
    public abstract record CoreEntity : CoreEntity<Guid> { }
}
