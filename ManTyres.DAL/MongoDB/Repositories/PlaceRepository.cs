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
   public class PlaceRepository : BaseRepository<Place>, IPlaceRepository
   {
      private readonly IConfiguration _configuration;
      private readonly ILogger<PlaceRepository> _logger;

      public PlaceRepository(IConfiguration configuration, ILogger<PlaceRepository> logger) : base(configuration, logger)
      {
         _configuration = configuration;
         _logger = logger;
      }

      public async Task<List<Place>> GetWhereIsNull()
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.Locality == null).ToListAsync();
      }

      public async Task<List<Place>> GetByPlacesId(string[] places_id)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         List<Place> result = new List<Place>();
         List<Place> no_photos = new List<Place>();

         foreach (var place_id in places_id)
         {
            var place = await Collection.Find(_ => _.Google_Place_Id == place_id).FirstOrDefaultAsync();
            if (place.Google_Photos != null && place.Google_Photos.Length == 0)
               no_photos.Add(place);
            else
               result.Add(place);
         }
         result.AddRange(no_photos);
         return result;
      }

      public async Task<bool> ExistByName(string name)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.Name == name).AnyAsync();
      }

      public async Task<bool> InsertMany(List<Place> entity)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         if (entity == null)
            throw new ArgumentNullException("entity");
         foreach (var place in entity)
         {
            place.Id = new ObjectId();
            place.CreatedAt = DateTime.UtcNow;
            place.UpdatedAt = DateTime.UtcNow;
         }
         await Collection.InsertManyAsync(entity);
         return true;
      }
   }
}
