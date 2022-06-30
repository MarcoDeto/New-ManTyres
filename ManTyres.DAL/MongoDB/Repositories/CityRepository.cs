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
   public class CityRepository: ICityRepository
   {
      public readonly IMongoCollection<City> Collection;
      private readonly IConfiguration _configuration;
      private readonly ILogger<CityRepository> _logger;

      public CityRepository(IConfiguration configuration, ILogger<CityRepository> logger)
      {
         Collection = GetCollection<City>();
			_configuration = configuration;
         _logger = logger;
      }

      public async Task<long> Count()
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.Name != "").CountDocumentsAsync();
      }

      public async Task<long> CountByISO(string ISO)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         if (ISO.Length == 2)
            return await Collection.Find(_ => _.ISO2 == ISO.ToUpper()).CountDocumentsAsync();
         
         return await Collection.Find(_ => _.ISO3!.StartsWith(ISO.ToUpper())).CountDocumentsAsync();
      }

      public async Task<List<City>> GetAll()
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.Name != "").ToListAsync();
      }

      public async Task<List<City>> GetByISO(string ISO)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         if (ISO.Length == 2)
            return await Collection.Find(_ => _.ISO2 == ISO.ToUpper()).ToListAsync();
         
         return await Collection.Find(_ => _.ISO3!.StartsWith(ISO.ToUpper())).ToListAsync();
      }

      public async Task<bool> InsertList(List<City> entity)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         if (entity == null)
            throw new ArgumentNullException("entity");
         foreach (var item in entity)
         {
            item.Id = new ObjectId();
         }
         await Collection.InsertManyAsync(entity);
         return true;
      }

      private IMongoDatabase GetDatabase()
      {
         var client = new MongoClient("mongodb+srv://dev:ManTyres@mantyres.fwdxdp6.mongodb.net/?retryWrites=true&w=majority");
         return client.GetDatabase("ManTyres");
      }

      private IMongoCollection<T> GetCollection<T>()
      {
         string collectionName = "Cities";
         var db = GetDatabase();
         return db.GetCollection<T>(collectionName);
      }
   }
}
