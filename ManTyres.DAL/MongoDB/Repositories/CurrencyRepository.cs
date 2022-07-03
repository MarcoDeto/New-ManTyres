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
   public class CurrencyRepository: ICurrencyRepository
   {
      public readonly IMongoCollection<Currency> Collection;
      private readonly IConfiguration _configuration;
      private readonly ILogger<CurrencyRepository> _logger;

      public CurrencyRepository(IConfiguration configuration, ILogger<CurrencyRepository> logger)
      {
         Collection = GetCollection<Currency>();
			_configuration = configuration;
         _logger = logger;
      }

      public async Task<long> Count()
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.Name != "").CountDocumentsAsync();
      }

      public async Task<List<Currency>> GetAll()
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.Name != "").ToListAsync();
      }

      public async Task<bool> InsertList(List<Currency> entity)
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
         string collectionName = "Currencies";
         var db = GetDatabase();
         return db.GetCollection<T>(collectionName);
      }
   }
}
