using Microsoft.AspNetCore.Identity;

namespace BLOG.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
