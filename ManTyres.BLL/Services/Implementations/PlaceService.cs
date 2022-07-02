using System.Net;
using AutoMapper;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using ManTyres.DAL.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;
using Microsoft.Extensions.Logging;

namespace ManTyres.BLL.Services.Implementations
{
   public class PlaceService : BaseService<PlaceDTO, IPlaceRepository, Place>, IPlaceService
   {
      private readonly IPlaceRepository _placeRepository;
      private readonly IMapper _mapper;
      private readonly ILogger<PlaceService> _logger;

      public PlaceService(IPlaceRepository placeRepository, IMapper mapper, ILogger<PlaceService> logger) : base(placeRepository, mapper, logger)
      {
         _mapper = mapper;
         _placeRepository = placeRepository;
         _logger = logger;
         _logger.LogDebug("ctor");
      }

      public async Task<Response<List<PlaceDTO>>> GetWhereIsNull()
      {
         return base.GetResponse(await _placeRepository.GetWhereIsNull());
      }

      public async Task<Response<List<PlaceDTO>>> GetNear(double LAT, double LNG)
      {
         var result = await _placeRepository.GetNear(LAT, LNG);
         return base.GetResponse(result);
      }

      public async Task<Response<List<PlaceDTO>>> GetByPlacesId(string[] places_id)
      {
         var result = await _placeRepository.GetByPlacesId(places_id);
         if (result != null && result.Count() > 0)
         {
            return new Response<List<PlaceDTO>>(result.ConvertAll(_mapper.Map<PlaceDTO>), result.Count(), HttpStatusCode.OK, null);
         }
         return new Response<List<PlaceDTO>>(new List<PlaceDTO>(){}, 0, HttpStatusCode.NoContent, null);
      }

      public async Task<Response<bool>> AddPlace(PlaceDTO place)
      {
         if (await _placeRepository.ExistByAddress(place.Address!))
         {
            return new Response<bool>(false, 0, HttpStatusCode.Conflict, "AlreadyExist");
         }
         else
         {
            var result = await _placeRepository.Insert(_mapper.Map<Place>(place));
            if (await _placeRepository.ExistByAddress(place.Address!))
            {
               return new Response<bool>(false, 0, HttpStatusCode.InternalServerError, "InternalServerError");
            }
            else
            {
               return new Response<bool>(true, 1, HttpStatusCode.OK, "Success");
            }
         }
      }

      public async Task<Response<bool>> AddPlaces(List<PlaceDTO> places)
      {
         var count = await _placeRepository.Count();
         foreach (var place in places)
         {
            if (await _placeRepository.ExistByName(place.Name))
            {
               continue;
            }
            await _placeRepository.Insert(_mapper.Map<Place>(place));
         }
         if (count == await _placeRepository.Count()) {
            return new Response<bool>(false, 0, HttpStatusCode.NoContent, "NoContent");
         }
         return new Response<bool>(true, 1, HttpStatusCode.OK, "Success");
      }
   }
}
