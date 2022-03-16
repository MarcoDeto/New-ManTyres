using ManTyres.COMMON.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface IClientiService
	{
		Task<Response<ClientiDTO>> GetById(int id);
		Task<Response<List<ClientiDTO>>> GetAll();
		Task<Response<List<ClientiDTO>>> GetAll(int skip, int take, bool orderByName, string filter = null);
		Task<Response<bool>> Add(ClientiDTO data);
		Task<Response<bool>> AddList(List<ClientiDTO> data);
		Task<Response<bool>> Update(ClientiDTO data);
		//Task<Response<bool>> Deactivate(int id);
	}
}
