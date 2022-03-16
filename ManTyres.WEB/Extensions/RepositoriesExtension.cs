using ManTyres.DAL.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Repositories;

namespace ManTyres.WEB.Extensions
{
	public static class RepositoriesExtension
   {
      public static IServiceCollection LoadRepositories(this IServiceCollection services)
      {
         services.AddScoped<IUserRepository, UserRepository>();
         return services;
      }
   }
}
