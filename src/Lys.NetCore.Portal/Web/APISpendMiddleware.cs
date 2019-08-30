using Lys.NetCore.Infrastructure.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lys.NetCore.Portal.Web
{
    public class APISpendMiddleware
    {
        private readonly RequestDelegate m_Next;
        private readonly ILogger m_Logger;

        public APISpendMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            m_Next = next;
            m_Logger = loggerFactory.CreateLogger("APISpend");
        }
        
        public async Task Invoke(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            context.Response.OnStarting(() =>
            {
                var spend = sw.ElapsedMilliseconds;
                if (spend > 2000)
                {
                    context.Items.TryGetValue(nameof(MySession), out var upaSession);
                    var requestId = ((MySession)upaSession)?.SessionId;
                    m_Logger.LogWarning($"响应开始，{requestId}，请求耗时：{spend}");
                }

                sw.Restart();
                return Task.CompletedTask;
            });

            context.Response.OnCompleted(() =>
            {
                var spend = sw.ElapsedMilliseconds;
                if (spend > 100)
                {
                    context.Items.TryGetValue(nameof(MySession), out var upaSession);
                    var requestId = ((MySession)upaSession)?.SessionId;
                    m_Logger.LogWarning($"响应结束，{requestId}，响应耗时：{sw.ElapsedMilliseconds}");
                }

                sw.Stop();
                return Task.CompletedTask;
            });

            await m_Next.Invoke(context);
        }
    }
}
