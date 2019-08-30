using System.IO;

namespace Lys.NetCore.Common
{
    /// <summary>
    /// 常量类
    /// </summary>
    public static class MyConstants
    {
        public static class Paths
        {
            public static readonly string FFmpegFolder = Path.Combine(Directory.GetCurrentDirectory(), "FFmpeg");
        }

        public static class Errors
        {
            public const string RequiredErrorMessage = "'{0}'参数不能为空";
            public const string StringLengthErrorMessage = "'{0}'最长支持'{1}'个字符";
            public const string RegularExpressionErrorMessage = "'{0}'参数格式不对";

            public const string EmptyFile = "空文件";
            public const string ConvertFileError = "文件转码失败";
            public const string RequestFileAPIFail = "FileAPI请求失败";
        }

        public static class Validations
        {
            public const int MobileStringLength = 11;
        }

        public static class DateTimeFormatter
        {
            public static readonly string HyphenLongDateTime = "yyyy-MM-dd HH:mm:ss";
            public static readonly string FullHyphenLongDateTime = "yyyy-MM-dd HH:mm:ss.ffffff";
        }
    }
}