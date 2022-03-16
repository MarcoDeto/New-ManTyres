using ManTyres.BLL.Services.Implementations;
using ManTyres.BLL.Services.Interfaces;

namespace TeqLinkPortal.API.Extensions
{
   public static class ServicesExtension
   {
      public static IServiceCollection LoadServices(this IServiceCollection services)
      {
         services.AddScoped<IUserService, UserService>();
         return services;
      }
   }
}
