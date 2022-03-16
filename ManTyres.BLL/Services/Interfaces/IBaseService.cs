using ManTyres.DAL.Infrastructure.MongoDB.Interfaces;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface IBaseService<T, U, V>
		  where T : class // DTO
		  where V : class // DBModel
		  where U : IBaseRepository<V> //IRepository
	{
		//Response<T> ModelStateControl(ModelStateDictionary mState);
		Task<Response<List<T>>> Get(int skip, int limit);
		Task<Response<T>> Get(string id);
		//Task<List<T>> GetFiltered(List<V> filteredList);
		Task<Response<bool>> PostReturnBool(T req);
		Task<Response<T>> PostReturnObj(T req);
		Task<Response<T>> Put(T req);
		Task<Response<bool>> Deactive(string id);
		Task<Response<bool>> Delete(string id);
	}
}
