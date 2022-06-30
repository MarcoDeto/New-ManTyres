using ManTyres.COMMON.DTO;
using ManTyres.DAL.Infrastructure.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;

namespace ManTyres.DAL.MongoDB.Interfaces
{
   public interface ICarRepository : IBaseRepository<Car>
   {
      Task<bool> IsAlreadyExist(Car entity, string wheel_size);
      Task<bool> InsertMany(List<Car> entity);
   }
}
