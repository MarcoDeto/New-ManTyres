using ManTyres.BLL.Services;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Tyre.WSL.Controllers
{
   [ApiController]
   [Route("api/[controller]/[action]")]
   public class PlaceController : ControllerBase
   {
      private readonly IPlaceService _placeService;
      private readonly IExcelService _excelService;

      private readonly ILogger<PlaceController> _logger;

      public PlaceController(IPlaceService placeService, IExcelService excelService, ILogger<PlaceController> logger)
      {
         _placeService = placeService;
         _excelService = excelService;
         _logger = logger;
      }

      [HttpPost]
      public async Task<ActionResult> AddPlace([FromBody] PlaceDTO place)
      {
         try
         {
            var response = await _placeService.AddPlace(place);
            return StatusCode((int)response.Code, response);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      [HttpPost]
      public async Task<ActionResult> AddPlaces([FromBody] List<PlaceDTO> places)
      {
         try
         {
            var response = await _placeService.AddPlaces(places);
            return StatusCode((int)response.Code, response);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      [HttpGet]
      public async Task<ActionResult> GetPlaces()
      {
         try
         {
            var result = await _placeService.GetWhereIsNull();
            return StatusCode((int)result.Code, result);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      [HttpGet]
      public async Task<ActionResult> GetNear(double LAT, double LNG)
      {
         try
         {
            var result = await _placeService.GetNear(LAT, LNG);
            return StatusCode((int)result.Code, result);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      [HttpPatch]
      public async Task<ActionResult> GetByPlacesId([FromBody] string[] places_id)
      {
         try
         {
            var result = await _placeService.GetByPlacesId(places_id);
            return StatusCode((int)result.Code, result);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      [HttpGet]
      public async Task<ActionResult> Get(string Id)
      {
         try
         {
            var result = await _placeService.Get(Id);
            return StatusCode((int)result.Code, result);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      [HttpGet]
      public async Task<ActionResult> UpdatePlaces()
      {
         try
         {
            var places = await _placeService.Get(0, 0);
            foreach (var place in places.Content)
            {
               place.ISO2 = "IT";
               await _placeService.Put(place);
            }
            return StatusCode((int)200, new Response<bool>(true, places.Count, HttpStatusCode.OK, null));
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      [HttpPut]
      public async Task<ActionResult> UpdatePlace([FromBody] PlaceDTO place)
      {
         try
         {
            var result = await _placeService.Put(place);
            return StatusCode((int)result.Code, result);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }
   }
}
