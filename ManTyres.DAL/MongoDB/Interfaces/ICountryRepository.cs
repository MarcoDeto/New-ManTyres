
using ManTyres.DAL.Infrastructure.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;

namespace ManTyres.DAL.MongoDB.Interfaces
{
   public interface ICountryRepository
   {
      Task<long> Count();
      Task<List<Country>> GetAll();
      Task<Country> GetByISO(string ISO);
      Task<bool> InsertList(List<Country> entity);
   }
}
