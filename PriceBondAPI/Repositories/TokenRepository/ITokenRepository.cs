using Microsoft.AspNetCore.Identity;

namespace PriceBondAPI.Repositories.TokkenRepository
{
    public interface ITokenRepository
    {
       string CreateJWTToken(IdentityUser user,List<string> roles);
    }
}
