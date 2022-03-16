using ManTyres.COMMON.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface IRoleService
	{
		Task<Response<List<RoleDTO>>> GetAll();
		Task<Response<List<RoleDTO>>> GetAll(int skip, int take);
		Task<Response<List<RoleDTO>>> GetRolesDisponibiliByUserId(string userId);
		Task<Response<RoleDTO>> GetById(string id);
		Task<Response<RoleDTO>> GetByName(string name);
		Task<Response<bool>> Create(string name);
		Task<Response<bool>> Update(RoleDTO role);
		Task<Response<bool>> Delete(string roleId);
		Task<Response<bool>> Deactive(string roleId);
		Task<Response<bool>> Reactive(string roleId);
		Task<Response<IList<string>>> GetRolesUser(string userId);
		Task<Response<bool>> AddRoleUser(UserRoleDTO user);
		Task<Response<bool>> DelRoleUser(UserRoleDTO user);
	}
}
