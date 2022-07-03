using System.Net;
using ManTyres.BLL.Services;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace Tyre.WSL.Controllers
{
   [ApiController]
   [Route("api/[controller]/[action]")]
   public class CountryController : ControllerBase
   {
      private readonly ICountryService _service;
      private readonly IExcelService _excelService;
      private readonly ILogger<CountryController> _logger;

      public CountryController(ICountryService service, IExcelService excelService, ILogger<CountryController> logger)
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

      [HttpGet]
      public async Task<IActionResult> GetByISO(string ISO)
      {
         try
         {
            var response = await _service.GetByISO(ISO);
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
               var response = await _excelService.ImportCities(stream);
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

      [HttpPut]
      public async Task<IActionResult> Update()
      {
         try
         {
            var response = await _service.Update();
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
