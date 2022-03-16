using ManTyres.BLL.Services;
using ManTyres.COMMON.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface IStagioniService
	{
		Task<Response<List<StagioniDTO>>> GetAllStagioni();
		Task<Response<bool>> AddStagioni();
	}
}
