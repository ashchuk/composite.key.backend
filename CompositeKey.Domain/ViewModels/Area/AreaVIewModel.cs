using System;
using System.ComponentModel.DataAnnotations;

namespace CompositeKey.Domain.ViewModels
{
    public class AreaViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
