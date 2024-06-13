using Microsoft.AspNetCore.Identity;

namespace CodePulse.API.Repositories.Interface
{
    public interface ITokeRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
