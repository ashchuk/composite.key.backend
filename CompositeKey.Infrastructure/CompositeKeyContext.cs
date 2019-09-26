using CompositeKey.Infrastructure.Map;
using CompositeKey.Model;
using CompositeKey.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompositeKey.Infrastructure
{
    public class CompositeKeyContext : IdentityDbContext<User, Role, string>
    {
        public CompositeKeyContext(DbContextOptions<CompositeKeyContext> options)
            : base(options)
        {
        }

        public DbSet<Area> Areas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.AddConfiguration(new AreaMap());

            builder.UseOpenIddict();
        }
    }
}
