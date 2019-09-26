using Microsoft.Extensions.DependencyInjection;
using System;
using CompositeKey.AutoMapper;

namespace CompositeKey.Services
{
    public static class AutoMapperService
    {
        public static void CreateAutoMapper(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var config = AutoMapperConfiguration.Configure();
            services.AddSingleton(sg => config.CreateMapper());
        }
    }
}
