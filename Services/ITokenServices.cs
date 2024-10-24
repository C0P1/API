namespace API.Services;
using API.Entitites;

public interface ITokenServices
{
    string CreateToken(AppUser user);
}