using System.IO;

namespace Common
{
    public static class MyConstants
    {
        public static class Paths
        {
            public static readonly string FFmpegFolder = Path.Combine(Directory.GetCurrentDirectory(), "Publish", "FFmpeg");
        }

        public static class Errors
        {
            public const string CallFileError = nameof(CallFileError);
        }
    }
}