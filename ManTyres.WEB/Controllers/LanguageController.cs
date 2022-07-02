using ManTyres.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Tyre.WSL.Controllers
{
   [ApiController]
   [Route("api/[controller]/[action]")]
   public class LanguageController : ControllerBase
   {
      private readonly ILanguageService _languageService;
      private readonly IExcelService _excelService;
      private readonly ILogger<LanguageController> _logger;

      public LanguageController(ILanguageService LanguageService, IExcelService excelService, ILogger<LanguageController> logger)
      {
         _languageService = LanguageService;
         _excelService = excelService;
         _logger = logger;
      }

      [HttpGet]
      public async Task<IActionResult> GetAll()
      {
         try
         {
            var response = await _languageService.GetAll();
            return StatusCode((int)response.Code, response);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      [HttpGet]
      public async Task<IActionResult> GetByCode(string ISO)
      {
         try
         {
            var response = await _languageService.GetByCode(ISO);
            return StatusCode((int)response.Code, response);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }
   }
}
