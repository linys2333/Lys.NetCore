using Lys.NetCore.Domain.Log;
using Lys.NetCore.Domain.Log.DTOs;
using Lys.NetCore.Infrastructure.Extensions;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Lys.NetCore.Portal.HttpHandlers
{
    public class LogAPIDelegatingHandler : DelegatingHandler
    {
        private readonly string m_logCode;

        public LogAPIDelegatingHandler(string logCode)
        {
            m_logCode = logCode;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            var response = await base.SendAsync(request, cancellationToken);
            sw.Stop();

            var logService = IoCExtensions.GetService<LogService>();
            logService.LogAPIRequest(m_logCode, new APIRequestInfo
            {
                Method = request.Method.ToString(),
                Url = request.RequestUri.AbsoluteUri,
                Paras = request.Content == null ? null : await request.Content.ReadAsStringAsync(),
                Spend = (int)sw.ElapsedMilliseconds,
                IsSuccess = response.IsSuccessStatusCode
            });

            return response;
        }
    }
}
