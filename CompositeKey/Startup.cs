using System;
using System.Collections.Generic;
using System.Linq;
using CompositeKey.API.Base;
using CompositeKey.Infrastructure;
using CompositeKey.Infrastructure.Extensions;
using CompositeKey.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;

namespace CompositeKey
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IServiceProvider Services;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "API"}); });

            services.CreateAutoMapper();

            services.AddMvc()
                .AddApplicationPart(typeof(ApiController).Assembly)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var dbHelper = new DbConnectionHelper(Configuration);
            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<CompositeKeyContext>(options =>
                {
                    options.UseNpgsql(dbHelper.GetConnectionString(),
                        b =>
                        {
                            b.MigrationsAssembly(typeof(CompositeKeyContext).Assembly.GetName().Name);
                            b.ProvideClientCertificatesCallback(dbHelper.ProvideClientCertificatesCallback);
                            b.RemoteCertificateValidationCallback(
                                dbHelper.RemoteCertificateValidationCallback);
                        });
                    options.UseOpenIddict();
                })
                .BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, CompositeKeyContext context,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            env.ConfigureNLog("nlog.config");

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API"); });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            context.EnsureSeedData();
        }
    }
}