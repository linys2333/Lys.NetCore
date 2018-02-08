namespace Common.Configuration
{
    /// <summary>
    /// 身份验证服务配置
    /// </summary>
    public class AuthConfig
    {
        public string Authority { get; set; }

        public string ApiName { get; set; }

        public string TokenUrl { get; set; }

        public string RevocationUrl { get; set; }
    }
}
