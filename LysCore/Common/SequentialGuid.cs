using System;

namespace LysCore.Common
{
    /// <summary>
    /// 有序GUID
    /// </summary>
    public static class SequentialGuid
    {
        public static Guid NewGuid()
        {
            var array = Guid.NewGuid().ToByteArray();
            var dateTime = new DateTime(1900, 1, 1);
            var now = DateTime.Now;
            var timeSpan = new TimeSpan(now.Ticks - dateTime.Ticks);
            var timeOfDay = now.TimeOfDay;
            var bytes = BitConverter.GetBytes(timeSpan.Days);
            var bytes2 = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));

            Array.Reverse(bytes);
            Array.Reverse(bytes2);
            Array.Copy(bytes, bytes.Length - 2, array, array.Length - 6, 2);
            Array.Copy(bytes2, bytes2.Length - 4, array, array.Length - 4, 4);
            
            return new Guid(array);
        }
    }
}
