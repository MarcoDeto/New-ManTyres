using ManTyres.BLL.Services;
using ManTyres.COMMON.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface IFirstConnectionService
	{
		Task<Response<SetupDTO>> GetSetup();
		Task<Response<bool>> AddFirstSede(SediDTO sede);
	}
}
