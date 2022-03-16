using ManTyres.BLL.Services;
using ManTyres.COMMON.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface IVeicoliService
	{
		Task<Response<VeicoliDTO>> GetById(int id);
		Task<Response<List<VeicoliDTO>>> GetByClienteId(int clienteId);
		Task<Response<List<VeicoliDTO>>> GetAllForSelectList();
		Task<Response<List<VeicoliDTO>>> GetAll();
		Task<Response<List<VeicoliDTO>>> GetAll(int skip, int take, bool orderByTarga, string targa);
		Task<Response<List<VeicoliDTO>>> GetAllDeleted();
		Task<Response<bool>> Add(VeicoliDTO Veicolo);
		Task<Response<bool>> AddList(List<VeicoliDTO> data);
		Task<Response<bool>> Update(VeicoliDTO Veicolo);
		//Task<Response<bool>> Deactivate(int id);
		//Task<Response<bool>> Reactivate(int id);
	}
}
