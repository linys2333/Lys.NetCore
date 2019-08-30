using Lys.NetCore.Infrastructure.Extensions;
using System.Diagnostics;

namespace Lys.NetCore.Infrastructure.FFmpeg
{
    public abstract class FFmpegBase : IFFmpeg
    {
        /// <summary>
        /// FFmpeg命令路径
        /// </summary>
        public abstract string FFmpegPath { get; }

        /// <summary>
        /// 系统命令行路径
        /// </summary>
        public abstract string CmdName { get; }

        /// <summary>
        /// FFmpeg命令参数
        /// </summary>
        public abstract string Arguments { get; }

        public virtual string Convert(string inputPath, string outputPath)
        {
            // -i表示输入文件；-y表示强制转换，覆盖已有文件
            // 更多命令见官方文档
            var cmd = $@"{FFmpegPath} -i {inputPath} -y {outputPath}";
            return Run(cmd);
        }

        protected virtual string Run(string cmd)
        {
            cmd = cmd.Replace(@"""", @"\""");

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = MyConstants.Paths.FFmpegFolder,
                    FileName = CmdName,
                    Arguments = $@"{Arguments} ""{cmd}""",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process.Start();

                var outStr = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                process.Close();

                return outStr;
            }
        }
    }
}
