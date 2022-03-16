using ManTyres.BLL.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface IExcelService
	{
		Response<byte[]> TracciatoClienti();
		Response<byte[]> TracciatoVeicoli();
		Response<byte[]> TracciatoAll();
		Task<Response<bool>> ImportClienti(Stream file);
		Task<Response<bool>> ImportVeicoli(Stream file);
		Task<Response<bool>> ImportAll(Stream file, int sedeId, string userId, string ci);
		Task<Response<byte[]>> ExportClienti();
		Task<Response<byte[]>> ExportVeicoli();
		Task<Response<byte[]>> ExportPneumatici(int sede, int stagione, bool orderByDesc);
		Task<Response<byte[]>> ExportInventario(int sede, int stagione, bool orderByDesc);
		Task<Response<byte[]>> ExportArchivio(int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc);
		Task<Response<byte[]>> ExportCestino(int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc);
	}
}
