using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManTyres.DAL.Infrastructure.MongoDB.Interfaces
{
	public interface IBaseRepository<T> where T : class
	{
		Task<long> Count();
		Task<List<T>> GetAll(int skip, int limit);
		Task<T> Get(string id);
		Task<T> Add(T entity);
		Task<bool> Insert(T entity);
		Task<T> Update(T entity);
		Task<bool> Deactive(string id);
		Task<bool> Delete(string id);
	}
}
