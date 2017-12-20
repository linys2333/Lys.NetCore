using System.IO;

namespace Common
{
    public static class IOExtensions
    {
        public static byte[] StreamToBytes(Stream stream)
        {
            if (stream == null)
            {
                return null;
            }

            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
    }
}
