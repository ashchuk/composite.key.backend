using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompositeKey.Model
{
    public class Area : EntityBase<string>
    {
        public Area()
        {
            CreatedOn = DateTimeOffset.Now;
        }

        [ConcurrencyCheck]
        public string Name { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string UpdatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual User CreatedBy { get; set; }
        [ForeignKey("UpdatedById")]
        public virtual User UpdatedBy { get; set; }
    }
}
