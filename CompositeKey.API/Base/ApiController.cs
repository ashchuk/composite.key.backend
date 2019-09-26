using CompositeKey.Infrastructure;
using CompositeKey.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CompositeKey.API.Base
{
    [Route("api/[controller]")]
    public abstract class ApiController : Controller
    {
        protected IConfiguration Configuration { get; set; }

        protected ApiController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        protected CompositeKeyContext GetContext(IConfiguration configuration)
        {
            var dbHelper = new DbConnectionHelper(Configuration);
            var ContextBuilder = new DbContextOptionsBuilder<CompositeKeyContext>();
            ContextBuilder.UseNpgsql(dbHelper.GetConnectionString(),
                        b =>
                        {
                            b.MigrationsAssembly(typeof(CompositeKeyContext).Assembly.GetName().Name);
                            b.ProvideClientCertificatesCallback(dbHelper.ProvideClientCertificatesCallback);
                            b.RemoteCertificateValidationCallback(
                                dbHelper.RemoteCertificateValidationCallback);
                        });
            return new CompositeKeyContext(ContextBuilder.Options);
        }
    }
}