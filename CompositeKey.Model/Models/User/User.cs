using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompositeKey.Model
{
    public class User : IdentityUser<string>
    {

        [InverseProperty("CreatedBy")]
        public virtual ICollection<Area> CreatedAreas { get; set; }

        [InverseProperty("UpdatedBy")]
        public virtual ICollection<Area> UpdatedAreas { get; set; } 

    }
}
