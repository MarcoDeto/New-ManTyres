using ManTyres.BLL.Services.Implementations;
using ManTyres.BLL.Services.Interfaces;

namespace TeqLinkPortal.API.Extensions
{
   public static class ServicesExtension
   {
      public static IServiceCollection LoadServices(this IServiceCollection services)
      {
         services.AddScoped<IUserService, UserService>();
         services.AddScoped<ILanguageService, LanguageService>();
         services.AddScoped<ICurrencyService, CurrencyService>();
         services.AddScoped<ICountryService, CountryService>();
         services.AddScoped<ICityService, CityService>();
         services.AddScoped<IPlaceService, PlaceService>();
         services.AddScoped<ICarService, CarService>();
         services.AddScoped<IExcelService, ExcelService>();
         return services;
      }
   }
}
