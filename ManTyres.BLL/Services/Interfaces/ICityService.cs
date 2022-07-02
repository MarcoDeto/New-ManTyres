


using ManTyres.BLL.Services.Implementations;
using ManTyres.COMMON.DTO;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface ICityService
	{
		Task<Response<long>> CountByISO(string ISO);
		Task<Response<List<CityDTO>>> GetAll();
		Task<Response<List<CityDTO>>> GetByISO(string ISO);
		Task<Response<bool>> Import(List<CityDTO> list);
	}
}