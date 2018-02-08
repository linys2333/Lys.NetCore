namespace Common.Interfaces
{
    public interface IFFmpeg
    {
        /// <summary>
        /// FFmpeg命令路径
        /// </summary>
        string FFmpegPath { get; }

        /// <summary>
        /// 系统命令行路径
        /// </summary>
        string CmdName { get; }

        /// <summary>
        /// FFmpeg命令参数
        /// </summary>
        string Arguments { get; }

        /// <summary>
        /// 格式转换
        /// </summary>
        /// <param name="inputPath">输入文件路径</param>
        /// <param name="outputPath">输出文件路径</param>
        /// <returns></returns>
        string Convert(string inputPath, string outputPath);
    }
}
