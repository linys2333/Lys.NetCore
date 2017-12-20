using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>
        {
            new ApiResource("api1")
        };

        public static IEnumerable<Client> GetClients() => new List<Client>
        {
            new Client
            {
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientId = "client",
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = {"api1"}
            },
            new Client
            {
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientId = "client.pwd",
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
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
