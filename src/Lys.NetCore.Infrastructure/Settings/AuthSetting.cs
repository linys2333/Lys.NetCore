namespace Lys.NetCore.Infrastructure.Settings
{
    /// <summary>
    /// 身份验证服务配置
    /// </summary>
    public class AuthSetting
    {
        public string Authority { get; set; }

        public string ApiName { get; set; }

        public string ApiScope { get; set; }

        public string TokenUrl { get; set; }

        public string RevocationUrl { get; set; }
    }
}
