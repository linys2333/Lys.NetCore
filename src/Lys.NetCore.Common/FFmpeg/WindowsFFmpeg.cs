namespace Lys.NetCore.Common.FFmpeg
{
    public class WindowsFFmpeg : FFmpegBase
    {
        public override string FFmpegPath => "ffmpeg.exe";

        public override string CmdName => "cmd.exe";

        public override string Arguments => "/c";
    }
}
