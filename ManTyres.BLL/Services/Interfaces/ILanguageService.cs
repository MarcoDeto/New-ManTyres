


using ManTyres.COMMON.DTO;

namespace ManTyres.BLL.Services.Interfaces
{
   public interface ILanguageService
   {
      Task<Response<List<LanguageDTO>>> GetAll();
      Task<Response<LanguageDTO>> GetByCode(string code);
      Task<Response<bool>> Import(List<LanguageDTO> list);
   }
}