using ManTyres.BLL.Services;
using ManTyres.COMMON.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface IInventarioService
	{
		Task<Response<List<InventarioDTO>>> GetAllPneumatici(int sede, int stagione, bool orderByDesc);
		Task<Response<List<InventarioDTO>>> GetAllInventario(int sede, int stagione, bool orderByDesc);
		Task<Response<List<InventarioDTO>>> GetInventario(int skip, int take, int sede, int stagione, bool orderByDesc, string targa = null);
		Task<Response<List<InventarioDTO>>> GetAllArchivio(int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc);
		Task<Response<List<InventarioDTO>>> GetArchivio(int skip, int take, int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc, string targa = null);
		Task<Response<bool>> DelFromArchivio(InventarioDTO item);
		Task<Response<List<InventarioDTO>>> GetAllCestino(int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc);
		Task<Response<List<InventarioDTO>>> GetCestino(int skip, int take, int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc, string targa = null);
		Task<Response<bool>> DelFromCestino(InventarioDTO item);
		Task<Response<bool>> RipristinaFromCestino(InventarioDTO item);
		Task<Response<bool>> RipristinaCestino();
		Task<Response<bool>> SvuotaCestino();
	}
}
