using ManTyres.COMMON.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface IPneumaticiService
	{
		Task<Response<List<InventarioDTO>>> GetLast2();
		Task<Response<List<InventarioDTO>>> GetByTarga(string targa);
		Task<Response<bool>> Add(InventarioDTO entity);
		Task<Response<bool>> AddList(List<InventarioDTO> archivio);
		Task<Response<bool>> GenerateData();
		Task<Response<bool>> Update(InventarioDTO entity);
		Task<Response<bool>> FineDeposito(InventarioDTO entity);
		Task<Response<bool>> InizioDeposito(InventarioDTO entity);
	}
}
