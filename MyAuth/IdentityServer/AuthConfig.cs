﻿using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace IdentityServer
{
    public class AuthConfig
    {
        private readonly IConfiguration m_Config;

        public AuthConfig(IConfigurationRoot config)
        {
            m_Config = config;
        }

        public IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>
        {
            new ApiResource("WebApiResource")
            {
                ApiSecrets = { new Secret("secret".Sha256()) },
                Scopes = new List<Scope>
                {
                    new Scope("webapi"),
                    new Scope("swagger")
                }
            }
        };

        public IEnumerable<Client> GetClients()
        {
            var clientSetting = new ClientCredentialSetting();
            m_Config.GetSection("ClientCredentialSetting").Bind(clientSetting);

            return new List<Client>
            {
                new Client
                {
                    // OAuth2 - 客户端验证模式
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientId = "debug",
                    ClientSecrets =
                    {
                        new Secret("debug".Sha256())
                    },
                    AllowedScopes = { "swagger" },
                    AllowedCorsOrigins = clientSetting.AllowedCorsOrigins,
                    // 设置为引用类型，才能在API端使用RevokeAccessTokenAsync注销token
                    AccessTokenType = AccessTokenType.Reference,
                    // token有效期，AccessTokenType=Reference下设置才有效
                    AccessTokenLifetime = 86400  // 一天
                },
                new Client
                {
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientId = "client",
                    ClientSecrets =
                    {
                        new Secret("client.secret".Sha256())
                    },
                    AllowedScopes = { "webapi" },
                    AllowedCorsOrigins = clientSetting.AllowedCorsOrigins,
                    AccessTokenType = AccessTokenType.Reference,
                    AccessTokenLifetime = 604800  // 一星期
                },
                new Client
                {
                    // OAuth2 - 密码验证模式
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientId = "client.pwd",
                    ClientSecrets =
                    {
                        new Secret("client.pwd.secret".Sha256())
                    },
                    AllowedScopes = { "webapi" },
                    AllowedCorsOrigins = clientSetting.AllowedCorsOrigins
                }
            };
        }

        public static List<TestUser> GetUsers() => new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "test",
                Password = "test"
            }
        };
    }

    public class ClientCredentialSetting
    {
        public string[] AllowedCorsOrigins { get; set; }
    }
}
