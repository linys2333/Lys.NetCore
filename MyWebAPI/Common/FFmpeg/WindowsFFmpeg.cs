namespace Common.FFmpeg
{
    public class WindowsFFmpeg : BaseFFmpeg
    {
        public override string FFmpegPath => "ffmpeg.exe";

        public override string CmdName => "cmd.exe";

        public override string Arguments => "/c";
    }
}
