
using ManTyres.DAL.MongoDB.Models;

namespace ManTyres.DAL.MongoDB.Interfaces
{
   public interface ICurrencyRepository
   {
      Task<long> Count();
      Task<List<Currency>> GetAll();
      Task<bool> InsertList(List<Currency> entity);
   }
}
