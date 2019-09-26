using Microsoft.AspNetCore.Identity;

namespace CompositeKey.Model
{
    public class Role : IdentityRole<string>
    {
        public string Title { get; set; }
    }
}
