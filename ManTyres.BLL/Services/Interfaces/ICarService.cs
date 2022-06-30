using ManTyres.DAL.MongoDB.Models;
using ManTyres.DAL.MongoDB.Repositories;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface ICarService : IBaseService<CarDTO, CarRepository, Car>
	{
		Task<Response<bool>> IsAlreadyExist(CarDTO entity, string wheel_size);
		Task<Response<bool>> Addlist(List<CarDTO> entity);
	}
}
