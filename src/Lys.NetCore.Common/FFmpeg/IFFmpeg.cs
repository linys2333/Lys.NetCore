namespace Lys.NetCore.Common.FFmpeg
{
    public interface IFFmpeg
    {
        /// <summary>
        /// 格式转换
        /// </summary>
        /// <param name="inputPath">输入文件路径</param>
        /// <param name="outputPath">输出文件路径</param>
        /// <returns></returns>
        string Convert(string inputPath, string outputPath);
    }
}
