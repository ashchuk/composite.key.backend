using System;
using CompositeKey.Core.Constants;
using CompositeKey.Model;

namespace CompositeKey.Infrastructure.Extensions
{
    public static class DefaultRole
    {
        static DefaultRole()
        {
            Admin = new Role
            {
                Id = Guid.NewGuid().ToString(),
                Name = Roles.Admin,
                NormalizedName = Roles.Admin.ToUpper(),
                Title = "Administrator"
            };
            User = new Role
            {
                Id = Guid.NewGuid().ToString(),
                Name = Roles.User,
                NormalizedName = Roles.User.ToUpper(),
                Title = "User"
            };
        }

        public static readonly Role Admin;
        public static readonly Role User;
    }
}
