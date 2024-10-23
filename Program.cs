using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
var app = builder.Build();

// Configurar la linea de HTTP
app.UseCors(cors => cors.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200","https://localhost:4200"));
app.UseAuthorization();
app.MapControllers();
app.Run();
