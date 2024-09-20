using API.Entitites;

namespace API.Services;
public interface ITokenServices
{
    string CreateToken(AppUser user);   
}