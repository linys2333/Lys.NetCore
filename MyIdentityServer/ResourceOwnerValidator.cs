using IdentityModel;
using IdentityServer4.Validation;
using MyIdentityServer.Services;
using System.Threading.Tasks;

namespace MyIdentityServer
{
    public class ResourceOwnerValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            string userId = new UserService().Login(context.UserName, context.Password);
            if (!string.IsNullOrEmpty(userId))
            {
                context.Result = new GrantValidationResult(userId, OidcConstants.AuthenticationMethods.Password);
            }
        }
    }
}
