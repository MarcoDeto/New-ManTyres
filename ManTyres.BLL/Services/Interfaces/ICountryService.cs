using ManTyres.COMMON.DTO;

namespace ManTyres.BLL.Services.Interfaces
{
   public interface ICountryService
   {
      Task<Response<List<CountryDTO>>> GetAll();
      Task<Response<CountryDTO>> GetByISO(string ISO);
      Task<Response<bool>> Import(List<CountryDTO> list);
      Task<Response<bool>> Update();
   }
}