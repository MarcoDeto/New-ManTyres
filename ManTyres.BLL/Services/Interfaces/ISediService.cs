using ManTyres.BLL.Services;
using ManTyres.COMMON.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface ISediService
	{
		Task<Response<List<SediDTO>>> GetAllSedi();
		Task<Response<SediDTO>> GetById(int id);
		Task<Response<bool>> AddSede(SediDTO sede);
		Task<Response<bool>> UpdateSede(SediDTO sede);
		Task<Response<bool>> DeactivateSede(int id);
	}
}
