namespace API.Extensions;

using API.Data;
<<<<<<< HEAD
=======
using API.Helpers;
>>>>>>> Parcial04
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
<<<<<<< HEAD
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
=======
        services.AddScoped<IPhotoService,PhotoService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
>>>>>>> Parcial04
        return services;
    }
}