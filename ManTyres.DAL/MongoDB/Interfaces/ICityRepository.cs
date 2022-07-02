
using ManTyres.DAL.MongoDB.Models;

namespace ManTyres.DAL.MongoDB.Interfaces
{
   public interface ICityRepository
   {
      Task<long> Count();
      Task<long> CountByISO(string ISO);
      Task<List<City>> GetAll();
      Task<List<City>> GetByISO(string ISO);
      Task<bool> InsertList(List<City> entity);
   }
}
