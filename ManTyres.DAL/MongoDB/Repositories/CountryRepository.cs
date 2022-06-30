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
   public class CountryRepository : ICountryRepository
   {
      public readonly IMongoCollection<Country> Collection;
      private readonly IConfiguration _configuration;
      private readonly ILogger<CountryRepository> _logger;

      public CountryRepository(IConfiguration configuration, ILogger<CountryRepository> logger)
      {
         Collection = GetCollection<Country>();
         _configuration = configuration;
         _logger = logger;
      }

      public async Task<long> Count()
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.Name != "").CountDocumentsAsync();
      }

      public async Task<List<Country>> GetAll()
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.Name != "").ToListAsync();
      }

      public async Task<Country> GetByISO(string ISO)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         if (ISO.Length == 2)
            return await Collection.Find(_ => _.ISO2 == ISO).SingleOrDefaultAsync();
         
         return await Collection.Find(_ => _.ISO3!.StartsWith(ISO)).SingleOrDefaultAsync();
      }

      public async Task<bool> InsertList(List<Country> entity)
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
         string collectionName = "Countries";
         var db = GetDatabase();
         return db.GetCollection<T>(collectionName);
      }
   }
}
