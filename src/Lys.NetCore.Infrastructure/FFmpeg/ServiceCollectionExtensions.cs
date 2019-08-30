using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lys.NetCore.Infrastructure.FFmpeg
{
    public static partial class ServiceCollectionExtensions
    {
        public static void AddFFmpeg(this IServiceCollection services)
        {
            IFFmpeg ffmpeg;
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    ffmpeg = new LinuxFFmpeg();
                    break;
                case PlatformID.Win32NT:
                    ffmpeg = new WindowsFFmpeg();
                    break;
                default:
                    throw new PlatformNotSupportedException("不支持的系统");
            }
            services.AddSingleton(ffmpeg);
        }
    }
}
