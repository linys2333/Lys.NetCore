using IdentityModel;
using IdentityServer.Services;
using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class ResourceOwnerValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var userId = new UserService().Login(context.UserName, context.Password);
            if (!string.IsNullOrEmpty(userId))
            {
                context.Result = new GrantValidationResult(userId, OidcConstants.AuthenticationMethods.Password);
            }
        }
    }
}
