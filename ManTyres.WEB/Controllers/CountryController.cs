using ManTyres.BLL.Services;
using ManTyres.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Tyre.WSL.Controllers
{
   [ApiController]
   [Route("api/[controller]/[action]")]
   public class CountryController : ControllerBase
   {
      private readonly ICountryService _countryService;
      private readonly IExcelService _excelService;
      private readonly ILogger<CountryController> _logger;

      public CountryController(ICountryService countryService, IExcelService excelService, ILogger<CountryController> logger)
      {
         _countryService = countryService;
         _excelService = excelService;
         _logger = logger;
      }

      [HttpGet]
      public async Task<IActionResult> GetAllCountries()
      {
         try
         {
            var response = await _countryService.GetAll();
            return StatusCode((int)response.Code, response);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      [HttpGet]
      public async Task<IActionResult> GetByISO(string ISO)
      {
         try
         {
            var response = await _countryService.GetByISO(ISO);
            return StatusCode((int)response.Code, response);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      [HttpPost]
      public async Task<IActionResult> ImportCountries(IFormFile file)
      {
         if (file == null) { return BadRequest(new Response<Exception>(null, 0, HttpStatusCode.BadRequest, "File obbligatorio")); }
         try
         {
            using (var stream = new MemoryStream())
            {
               file.CopyTo(stream);
               var response = await _excelService.ImportCountries(stream);
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
      }
   }
}
