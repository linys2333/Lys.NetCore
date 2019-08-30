using System.Net.Http;
using System.Security.Authentication;

namespace Lys.NetCore.Portal.HttpHandlers
{
    public class HttpsClientHandler : HttpClientHandler
    {
        public HttpsClientHandler(string url)
        {
            if (url.StartsWith("https"))
            {
                ServerCertificateCustomValidationCallback =
                    ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;					
                SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
            }
        }
    }
}
