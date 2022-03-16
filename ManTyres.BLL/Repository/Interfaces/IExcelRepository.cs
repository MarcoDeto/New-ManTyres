using ManTyres.COMMON.DTO;
using System.Collections.Generic;
using System.IO;
using DataTable = System.Data.DataTable;

namespace ManTyres.BLL.Repository.Interfaces
{
	public interface IExcelRepository
	{
		DataTable ImportExcel(Stream file);
		byte[] ExportClienti(List<ClientiDTO> clienti);
		byte[] ExportVeicoli(List<VeicoliDTO> veicoli);
		byte[] ExportPneumatici(List<InventarioDTO> pneumatici);
	}
}
