using ManTyres.BLL.Repository.Implementations;
using ManTyres.BLL.Repository.Interfaces;
using ManTyres.DAL.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Repositories;

namespace ManTyres.WEB.Extensions
{
	public static class RepositoriesExtension
   {
      public static IServiceCollection LoadRepositories(this IServiceCollection services)
      {
         services.AddScoped<IUserRepository, UserRepository>();
         services.AddScoped<ILanguageRepository, LanguageRepository>();
         services.AddScoped<ICountryRepository, CountryRepository>();
         services.AddScoped<ICityRepository, CityRepository>();
         services.AddScoped<IPlaceRepository, PlaceRepository>();
         services.AddScoped<ICarRepository, CarRepository>();
         services.AddScoped<IExcelRepository, ExcelRepository>();
         
         return services;
      }
   }
}
