using ManTyres.BLL.Services;
using ManTyres.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Tyre.WSL.Controllers
{
   [ApiController]
   [Route("api/[controller]/[action]")]
   public class CurrencyController : ControllerBase
   {
      private readonly ICurrencyService _service;
      private readonly IExcelService _excelService;
      private readonly ILogger<CurrencyController> _logger;

      public CurrencyController(ICurrencyService service, IExcelService excelService, ILogger<CurrencyController> logger)
      {
         _service = service;
         _excelService = excelService;
         _logger = logger;
      }

      [HttpGet]
      public async Task<IActionResult> GetAll()
      {
         try
         {
            var response = await _service.GetAll();
            return StatusCode((int)response.Code, response);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      /* ERRORE: MANCA IL METODO EXCEL! (INUTILE)...

      [HttpPost]
      public async Task<IActionResult> ImportCurrencies(IFormFile file)
      {
         if (file == null) { return BadRequest(new Response<Exception>(null, 0, HttpStatusCode.BadRequest, "File obbligatorio")); }
         try
         {
            using (var stream = new MemoryStream())
            {
               file.CopyTo(stream);
               var response = await _excelService.ImportCurrencies(stream);
               switch (response.Code)
               {
                  case HttpStatusCode.OK: return Ok(response);

                  case HttpStatusCode.BadRequest: return BadRequest(response);

                  default: return NoContent();
               }
            }
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }*/
   }
}
