namespace API.Services;
using API.DataEntities;

public interface ITokenServices
{
    string CreateToken(AppUser user);
}