using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lys.NetCore.Portal.HttpHandlers
{
    public class AuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly string m_AppKey;
        private readonly string m_AppSecret;

        public AuthorizationDelegatingHandler(string appKey, string appSecret)
        {
            m_AppKey = appKey;
            m_AppSecret = appSecret;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", 
                $"{Convert.ToBase64String(Encoding.UTF8.GetBytes($"{m_AppKey}:{m_AppSecret}"))}");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
