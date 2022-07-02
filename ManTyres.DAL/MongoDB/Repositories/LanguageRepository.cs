using ManTyres.COMMON.Utils;
using ManTyres.DAL.MongoDB.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Language = ManTyres.DAL.MongoDB.Models.Language;

namespace ManTyres.DAL.MongoDB.Repositories
{
   public class LanguageRepository : ILanguageRepository
   {
      public readonly IMongoCollection<Language> Collection;
      private readonly IConfiguration _configuration;
      private readonly ILogger<LanguageRepository> _logger;

      public LanguageRepository(IConfiguration configuration, ILogger<LanguageRepository> logger)
      {
         Collection = GetCollection<Language>();
         _configuration = configuration;
         _logger = logger;
      }

      public async Task<long> Count()
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.Name != "").CountDocumentsAsync();
      }

      public async Task<List<Language>> GetAll()
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.Name != "").ToListAsync();
      }

      public async Task<Language> GetByCode(string code)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.Code == code.ToUpper()).SingleOrDefaultAsync();
      }

      public async Task<bool> InsertList(List<Language> entity)
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
         string collectionName = "Languages";
         var db = GetDatabase();
         return db.GetCollection<T>(collectionName);
      }
   }
}
