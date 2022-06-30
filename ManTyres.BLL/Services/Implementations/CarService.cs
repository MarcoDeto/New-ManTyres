using System.Net;
using AutoMapper;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using ManTyres.DAL.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;
using Microsoft.Extensions.Logging;

namespace ManTyres.BLL.Services.Implementations
{
   public class CarService : BaseService<CarDTO, ICarRepository, Car>, ICarService
   {
      private readonly ICarRepository _carRepository;
      private readonly IMapper _mapper;
      private readonly ILogger<CarService> _logger;

      public CarService(ICarRepository carRepository, IMapper mapper, ILogger<CarService> logger) : base(carRepository, mapper, logger)
      {
         _mapper = mapper;
         _carRepository = carRepository;
         _logger = logger;
         _logger.LogDebug("ctor");
      }

      public async Task<Response<bool>> IsAlreadyExist(CarDTO entity, string wheel_size)
      {
         if (await _carRepository.IsAlreadyExist(_mapper.Map<Car>(entity), wheel_size))
         {
            return new Response<bool>(true, 0, HttpStatusCode.Conflict, "AlreadyExist");
         }
         return new Response<bool>(false, 0, HttpStatusCode.OK, null);
      }

      public async Task<Response<bool>> Addlist(List<CarDTO> entity)
      {
         var count = await _carRepository.Count();
         await _carRepository.InsertMany(entity.ConvertAll(_mapper.Map<Car>));
         var check = await _carRepository.Count();
         if (count == check) {
            return new Response<bool>(false, 0, HttpStatusCode.NoContent, "NoContent");
         }
         return new Response<bool>(true, check - count, HttpStatusCode.OK, "Success");
      }
   }
}
