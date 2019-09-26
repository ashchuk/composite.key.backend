using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CompositeKey.Infrastructure.Extensions
{
    public class DbConnectionHelper
    {
        private IConfiguration Configuration { get; }

        public DbConnectionHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GetConnectionString() =>
            new NpgsqlConnectionStringBuilder
            {
                Host = Configuration.GetSection("ConnectionString").GetValue<string>("Host"),
                Port = Configuration.GetSection("ConnectionString").GetValue<int>("Port"),
                SslMode = SslMode.Require,
                Username = Configuration.GetSection("ConnectionString").GetValue<string>("Username"),
                Database = Configuration.GetSection("ConnectionString").GetValue<string>("Database")
            }.ConnectionString;

        public void ProvideClientCertificatesCallback(X509CertificateCollection clientCerts)
        {
            // To be able to add a certificate with a private key included, we must convert it to
            // a PKCS #12 format. The following openssl command does this:
            // openssl pkcs12 -password pass: -inkey client.maxroach.key -in client.maxroach.crt -export -out client.maxroach.pfx
            // As of 2018-12-10, you need to provide a password for this to work on macOS.
            // See https://github.com/dotnet/corefx/issues/24225

            // Note that the password used during X509 cert creation below
            // must match the password used in the openssl command above.
            clientCerts.Add(new X509Certificate2(Configuration.GetSection("ConnectionString").GetValue<string>("ClientCertificatePath"),
                Configuration.GetSection("ConnectionString").GetValue<string>("CertSecret")));
        }

        // By default, .Net does all of its certificate verification using the system certificate store.
        // This callback is necessary to validate the server certificate against a CA certificate file.
        public bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate,
            X509Chain defaultChain, SslPolicyErrors defaultErrors)
        {
            X509Certificate2 caCert = new X509Certificate2(Configuration.GetSection("ConnectionString").GetValue<string>("CACertificatePath"));
            X509Chain caCertChain = new X509Chain();
            caCertChain.ChainPolicy = new X509ChainPolicy()
            {
                RevocationMode = X509RevocationMode.NoCheck,
                RevocationFlag = X509RevocationFlag.EntireChain
            };
            caCertChain.ChainPolicy.ExtraStore.Add(caCert);

            X509Certificate2 serverCert = new X509Certificate2(certificate);

            caCertChain.Build(serverCert);
            if (caCertChain.ChainStatus.Length == 0)
            {
                // No errors
                return true;
            }

            foreach (X509ChainStatus status in caCertChain.ChainStatus)
            {
                // Check if we got any errors other than UntrustedRoot (which we will always get if we don't install the CA cert to the system store)
                if (status.Status != X509ChainStatusFlags.UntrustedRoot)
                {
                    return false;
                }
            }

            return true;
        }
    }
}