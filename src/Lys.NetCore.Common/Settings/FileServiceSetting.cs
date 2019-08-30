namespace Lys.NetCore.Common.Settings
{
    /// <summary>
    /// 文件服务配置，用于上传/下载文件
    /// </summary>
    public class FileServiceSetting
    {
        public string BaseUrl { get; set; }

        public string Key { get; set; }

        public string Secret { get; set; }
    }
}
