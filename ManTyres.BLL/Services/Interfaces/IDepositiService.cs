using ManTyres.BLL.Services;
using ManTyres.COMMON.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface IDepositiService
	{
		Task<Response<List<DepositiDTO>>> GetAll();
		Task<Response<bool>> Add(DepositiDTO deposito);
		Task<Response<bool>> Update(DepositiDTO deposito);
	}
}
