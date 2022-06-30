using ManTyres.COMMON.DTO;
using ManTyres.DAL.Infrastructure.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;

namespace ManTyres.DAL.MongoDB.Interfaces
{
   public interface IUserRepository : IBaseRepository<User>
   {
      Task<User> GetByEmail(string email);
	  Task<bool> IsAlreadyExists(string email);
      Task<bool> CheckPassword(LoginDTO request); 
   }
}
