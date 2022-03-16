using ManTyres.BLL.Services;
using ManTyres.COMMON.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface IDashboardService
	{
		Response<int> TotalePneumatici();
		Response<int> TotaleVeicoli();
		Response<int> TotaleClienti();
		Response<int> TotaleUtenti();
		Task<Response<List<ChartDTO>>> ChartQuantitàPneumatici(int month, string ci);
		Task<Response<List<ChartDTO>>> ChartGlobalePneumatici(int month, string ci);
		Task<Response<List<ChartDTO>>> ChartQuantitàVeicoli(int month, string ci);
		Task<Response<List<ChartDTO>>> ChartGlobaleVeicoli(int month, string ci);
		Task<Response<List<ChartDTO>>> ChartQuantitàClienti(int month, string ci);
		Task<Response<List<ChartDTO>>> ChartGlobaleClienti(int month, string ci);
	}
}
