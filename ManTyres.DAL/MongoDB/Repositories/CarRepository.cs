using ManTyres.COMMON.DTO;
using ManTyres.COMMON.Utils;
using ManTyres.DAL.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ManTyres.DAL.MongoDB.Repositories
{
   public class CarRepository : BaseRepository<Car>, ICarRepository
   {
      private readonly IConfiguration _configuration;
      private readonly ILogger<CarRepository> _logger;

      public CarRepository(IConfiguration configuration, ILogger<CarRepository> logger) : base(configuration, logger)
      {
			_configuration = configuration;
         _logger = logger;
      }

      public async Task<bool> IsAlreadyExist(Car entity, string wheel_size)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());

         if (await Collection.Find(_ => _.Company == entity.Company && _.Model == entity.Model && _.Wheel_sizes.Contains(wheel_size)).AnyAsync()) {
            return true;
         }
         var car = await Collection.Find(_ => _.Model == entity.Model).SingleOrDefaultAsync();
         if (car != null) {
            if (car.Wheel_sizes != null && car.Wheel_sizes.Length > 0) {
               car.Wheel_sizes.Append(wheel_size);
            } else {
               string[] Wheel_sizes = new string[] {wheel_size};
               car.Wheel_sizes = Wheel_sizes;
            }
            await Collection.FindOneAndReplaceAsync(_ => _.Id == car.Id, car);
            return true;
         }
         return false;
      }

      public async Task<bool> InsertMany(List<Car> entity)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         if (entity == null)
            throw new ArgumentNullException("entity");
         foreach (var place in entity) {
            place.Id = new ObjectId();
            place.CreatedAt = DateTime.UtcNow;
            place.UpdatedAt = DateTime.UtcNow;
         }
         await Collection.InsertManyAsync(entity);
         return true;
      }
   }
}
