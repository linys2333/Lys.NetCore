using System.IO;

namespace Lys.NetCore.Common.FFmpeg
{
    public class LinuxFFmpeg : FFmpegBase
    {
        /// <summary>
        /// Linux下需要绝对路径，否则会识别为全局命令（除非系统本身已安装FFmpeg）
        /// </summary>
        public override string FFmpegPath => Path.Combine(MyConstants.Paths.FFmpegFolder, "ffmpeg");

        public override string CmdName => "/bin/bash";

        public override string Arguments => "-c";
    }
}
