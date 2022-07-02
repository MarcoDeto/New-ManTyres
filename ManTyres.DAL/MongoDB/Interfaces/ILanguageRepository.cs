
using ManTyres.DAL.MongoDB.Models;

namespace ManTyres.DAL.MongoDB.Interfaces
{
   public interface ILanguageRepository
   {
      Task<long> Count();
      Task<List<Language>> GetAll();
      Task<Language> GetByCode(string code);
      Task<bool> InsertList(List<Language> entity);
   }
}
