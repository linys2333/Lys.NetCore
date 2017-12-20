namespace Common.Interfaces
{
    public interface IFFmpeg
    {
        string FFmpegPath { get; }

        string CmdName { get; }

        string Arguments { get; }

        string Convert(string inputPath, string outputPath);
    }
}
