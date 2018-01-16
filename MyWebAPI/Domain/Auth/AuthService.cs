using Common.Configuration;
using IdentityModel.Client;
using LysCore.Common;
using LysCore.Exceptions;
using LysCore.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Domain.Auth
{
    public class AuthService : LysDomain
    {
        private readonly AuthConfig m_AuthConfig;

        public AuthService(IOptions<AuthConfig> authConfig)
        {
            m_AuthConfig = authConfig.Value;
        }
        
        public async Task<JObject> GetToken(string clientId, string secret)
        {
            Requires.NotNullOrEmpty(clientId, nameof(clientId));
            Requires.NotNullOrEmpty(secret, nameof(secret));
            
            var discoveryResponse = await DiscoveryClient.GetAsync(m_AuthConfig.Authority);
            var tokenUrl = discoveryResponse.IsError ? m_AuthConfig.TokenUrl : discoveryResponse.TokenEndpoint;
            var tokenClient = new TokenClient(tokenUrl, clientId, secret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(m_AuthConfig.ApiName);

            if (tokenResponse.IsError)
            {
                throw new HttpRequestException(tokenResponse.Error);
            }

            return tokenResponse.Json;
        }
        
        public async Task<JObject> RevokeToken(string clientId, string secret, string token)
        {
            Requires.NotNullOrEmpty(clientId, nameof(clientId));
            Requires.NotNullOrEmpty(secret, nameof(secret));
            Requires.NotNullOrEmpty(token, nameof(token));

            var discoveryResponse = await DiscoveryClient.GetAsync(m_AuthConfig.Authority);
            var revocationUrl = discoveryResponse.IsError ? m_AuthConfig.RevocationUrl : discoveryResponse.RevocationEndpoint;
            var tokenClient = new TokenRevocationClient(revocationUrl, clientId, secret);
            var tokenResponse = await tokenClient.RevokeAccessTokenAsync(token);

            if (tokenResponse.IsError)
            {
                throw new HttpRequestException(tokenResponse.Error);
            }

            return tokenResponse.Json;
        }
    }
}