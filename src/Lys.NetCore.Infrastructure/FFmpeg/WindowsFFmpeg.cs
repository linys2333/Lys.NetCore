namespace Lys.NetCore.Infrastructure.FFmpeg
{
    public class WindowsFFmpeg : FFmpegBase
    {
        public override string FFmpegPath => "ffmpeg.exe";

        public override string CmdName => "cmd.exe";

        public override string Arguments => "/c";
    }
}
