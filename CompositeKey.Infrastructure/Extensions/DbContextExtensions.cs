using System.Linq;
using CompositeKey.Model;

namespace CompositeKey.Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        public static void EnsureSeedData(this CompositeKeyContext context)
        {
            if (!context.Roles.Any())
                context.Roles.AddRange(DefaultRole.Admin, DefaultRole.User);
            if (!context.Users.Any())
                context.Users.Add(new User()
                {
                    UserName = "Admin"
                });
            context.SaveChanges();
        }
    }
}