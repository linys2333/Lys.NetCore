using Serilog;

namespace LysCore.Log
{
    public static class LysLog
    {
        public static ILogger Logger { get; set; }

        public static ILogger BizLogger { get; set; }
    }
}
