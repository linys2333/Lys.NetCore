using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace MyIdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>
        {
            new ApiResource("api1", "MyWebAPI")
        };

        public static IEnumerable<Client> GetClients() => new List<Client>
        {
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                // 用于认证的密码
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                // 客户端有权访问的范围（Scopes）
                AllowedScopes = {"api1"}
            }
        };

        public static List<TestUser> GetUsers() => new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "lys",
                Password = "123"
            }
        };
    }
}
