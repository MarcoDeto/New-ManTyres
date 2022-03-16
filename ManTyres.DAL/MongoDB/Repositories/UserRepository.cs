using ManTyres.COMMON.Utils;
using ManTyres.DAL.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ManTyres.DAL.MongoDB.Repositories
{
   public class UserRepository : BaseRepository<User>, IUserRepository
   {
      private readonly IConfiguration _configuration;
      private readonly ILogger<UserRepository> _logger;

      public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger) : base(configuration, logger)
      {
			_configuration = configuration;
         _logger = logger;
      }

      public async Task<User> GetByEmail(string email)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await base.Collection.Find(_ => _.Email == email).SingleOrDefaultAsync();
      }

      #region UTILITIES
      public async Task<bool> IsAlreadyExists(string email)
      {
         _logger.LogDebug(LoggerHelper.GetActualMethodName());
         return await base.Collection.Find(_ => _.Email == email).AnyAsync();
      }
      #endregion
   }
}
