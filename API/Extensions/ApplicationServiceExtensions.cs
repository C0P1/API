namespace API.Extensions;

using API.Data;
using API.Services;
using Microsoft.EntityFrameworkCore;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(opt => opt.UseSqlite(config.GetConnectionString("DefaultConnection")));
        services.AddCors();
        services.AddScoped<ITokenServices, TokenServices>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}