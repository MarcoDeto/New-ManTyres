using ManTyres.COMMON.DTO;

namespace ManTyres.BLL.Services.Interfaces
{
   public interface ICurrencyService
   {
      Task<Response<List<CurrencyDTO>>> GetAll();
      Task<Response<bool>> Import(List<CurrencyDTO> list);
   }
}