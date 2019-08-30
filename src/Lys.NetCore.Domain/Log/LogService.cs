using Lys.NetCore.Domain.Log.DTOs;
using Lys.NetCore.Domain.Log.Models;
using Lys.NetCore.Infrastructure.Extensions;
using Lys.NetCore.Infrastructure.Services;
using Lys.NetCore.Infrastructure.Settings;
using Lys.NetCore.Infrastructure.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Newtonsoft.Json;
using System;

namespace Lys.NetCore.Domain.Log
{
    public class LogService : ServiceBase
    {
        private readonly ILogger m_Logger;
        private readonly ProjectInfoSetting m_ProjectInfoSetting;
        private readonly IHttpContextAccessor m_HttpContextAccessor;

        private static readonly string m_OSName = Environment.OSVersion.Platform.ToString();
        private static readonly string m_OSVersion = Environment.OSVersion.VersionString;

        public LogService(ILogger logger, ProjectInfoSetting projectInfoSetting, IHttpContextAccessor httpContextAccessor)
        {
            m_Logger = logger;
            m_ProjectInfoSetting = projectInfoSetting;
            m_HttpContextAccessor = httpContextAccessor;
        }

        public MySession GetMySession()
        {
            m_HttpContextAccessor.HttpContext.Items.TryGetValue(nameof(MySession), out var session);
            return (MySession)session;
        }

        public string LogAPIRequest(string logCode, APIRequestInfo requestInfo)
        {
            Requires.NotNullOrEmpty(logCode, nameof(logCode));
            Requires.NotNull(requestInfo, nameof(requestInfo));

            var session = GetMySession();
            if (session == null)
            {
                return string.Empty;
            }

            var log = InitLog<RunningTimeLog>(session);
            log.LogCode = logCode;
            log.LogData = JsonConvert.SerializeObject(requestInfo);
            log.Spend = requestInfo.Spend;

            m_Logger.LogInformation(JsonConvert.SerializeObject(log));

            return log.SessionId;
        }

        public string LogException(Exception ex, string errorCode)
        {
            Requires.NotNullOrEmpty(errorCode, nameof(errorCode));

            var session = GetMySession();
            if (session == null)
            {
                return string.Empty;
            }

            var log = InitLog<ExceptionLog>(session);
            log.ErrorCode = errorCode;
            log.ErrorData = ex?.ToString() ?? string.Empty;

            m_Logger.LogError(ex, $"{errorCode}|{log.SessionId}");

            return log.SessionId;
        }

        private T InitLog<T>(MySession session) where T : LogBase, new()
        {
            return new T
            {
                ProjectCode = m_ProjectInfoSetting.ProjectCode,
                Version = m_ProjectInfoSetting.Version,
                OSName = m_OSName,
                OSVersion = m_OSVersion,
                ClientIP = session.ClientIP,
                LocalIP = session.LocalIP,
                SessionId = session.SessionId,
                Time = DateTime.Now.ToString(MyConstants.DateTimeFormatter.FullHyphenLongDateTime)
            };
        }
    }
}
