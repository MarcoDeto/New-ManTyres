using ManTyres.COMMON.Utils;
using ManTyres.DAL.Infrastructure.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ManTyres.DAL.MongoDB.Repositories
{
   public class BaseRepository<T> : IBaseRepository<T> where T : MongoDocument
   {
      public readonly IMongoCollection<T> Collection;
      private readonly ILogger<BaseRepository<T>> _logger;
		private readonly IConfiguration _configuration;

      public BaseRepository(IConfiguration configuration, ILogger<BaseRepository<T>> logger)
      {
			_configuration = configuration;
         Collection = GetCollection<T>();
         _logger = logger;
         _logger.LogDebug("ctor");
      }

      public async Task<long> Count()
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.IsDeleted == false).CountDocumentsAsync();
      }

      public async Task<List<T>> GetAll(int skip, int limit)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.IsDeleted == false).Skip(skip).Limit(limit).ToListAsync();
      }

      public async Task<T> Get(string id)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await Collection.Find(_ => _.Id == ObjectId.Parse(id)).SingleOrDefaultAsync();
      }

      public async Task<T> Add(T entity)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         if (entity == null)
            throw new ArgumentNullException("entity");
         entity.Id = new ObjectId();
         entity.CreatedAt = DateTime.UtcNow;
         entity.UpdatedAt = DateTime.UtcNow;
         await Collection.InsertOneAsync(entity);
         return await Get(entity.Id.ToString());
      }

      public async Task<bool> Insert(T entity)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         if (entity == null)
            throw new ArgumentNullException("entity");
         entity.Id = new ObjectId();
         entity.CreatedAt = DateTime.UtcNow;
         entity.UpdatedAt = DateTime.UtcNow;
         await Collection.InsertOneAsync(entity);
         return await Collection.Find(_ => _.Id == entity.Id).AnyAsync();
      }

      public async virtual Task<T> Update(T entity)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         if (entity == null)
            throw new ArgumentNullException("entity");
         entity.UpdatedAt = DateTime.UtcNow;
         return await Collection.FindOneAndReplaceAsync(_ => _.Id == entity.Id, entity);
      }

      public async virtual Task<bool> Deactivate(string id)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         var entity = await Collection.Find(_ => _.Id == ObjectId.Parse(id)).SingleOrDefaultAsync();
         if (entity == null)
            return false;
         entity.IsDeleted = true;
         entity.UpdatedAt = DateTime.UtcNow;
         return await Collection.FindOneAndReplaceAsync(_ => _.Id == entity.Id, entity) != null ? true : false;
      }

      public async virtual Task<bool> Reactivate(string id)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         var entity = await Collection.Find(_ => _.Id == ObjectId.Parse(id)).SingleOrDefaultAsync();
         if (entity == null)
            return false;
         entity.IsDeleted = false;
         entity.UpdatedAt = DateTime.UtcNow;
         return await Collection.FindOneAndReplaceAsync(_ => _.Id == entity.Id, entity) != null ? true : false;
      }

      public async virtual Task<bool> Delete(string id)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         var result = await Collection.DeleteOneAsync(_ => _.Id == ObjectId.Parse(id));
         if (result.DeletedCount == 0)
            return false;
         return true;
      }

      private IMongoDatabase GetDatabase()
      {
         var test = _configuration.GetConnectionString("MongoDB");
         var client = new MongoClient(test);
         return client.GetDatabase("ManTyres");
      }

      private IMongoCollection<T> GetCollection<T>()
      {
         var collectionName = typeof(T).Name + 's';
         //_logger.LogTrace("GetCollection(), collectionName: {0}", collectionName);
         var db = GetDatabase();
         return db.GetCollection<T>(collectionName);
      }

      private IQueryable<T> GetQueryableCollection<T>()
      {
         var collectionName = typeof(T).Name;
         _logger.LogTrace("GetCollection(), collectionName: {0}", collectionName);
         var db = GetDatabase();
         return db.GetCollection<T>(collectionName).AsQueryable();
      }
   }
}
