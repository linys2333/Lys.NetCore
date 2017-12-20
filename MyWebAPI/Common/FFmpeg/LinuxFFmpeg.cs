using System.IO;

namespace Common.FFmpeg
{
    public class LinuxFFmpeg : BaseFFmpeg
    {
        public override string FFmpegPath => Path.Combine(MyConstants.Paths.FFmpegFolder, "ffmpeg");

        public override string CmdName => "/bin/bash";

        public override string Arguments => "-c";
    }
}
