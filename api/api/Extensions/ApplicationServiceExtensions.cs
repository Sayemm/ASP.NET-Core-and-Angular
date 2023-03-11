using api.Data;
using api.Interfaces;
using api.Services;
using Microsoft.EntityFrameworkCore;

namespace api.Extensions
{
  public static class ApplicationServiceExtensions
  {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
      services.AddScoped<ITokenService, TokenService>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddDbContext<DataContext>(options =>
      {
        options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
      });

      return services;
    }
  }
}
