using System.Net;
using AutoMapper;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using ManTyres.DAL.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;
using Microsoft.Extensions.Logging;

namespace ManTyres.BLL.Services.Implementations
{
   public class CurrencyService : ICurrencyService
   {
      private readonly ICurrencyRepository _repository;
      private readonly IMapper _mapper;
      private readonly ILogger<CurrencyService> _logger;

      public CurrencyService(ICurrencyRepository repository, IMapper mapper, ILogger<CurrencyService> logger)
      {
         _mapper = mapper;
         _repository = repository;
         _logger = logger;
         _logger.LogDebug("ctor");
      }

      public async Task<Response<List<CurrencyDTO>>> GetAll()
      {
         _logger.LogDebug("Get (list)");
         var result = await _repository.GetAll();
         _logger.LogTrace($"200. Returned items: {result.Count()}");
         result = result.OrderBy(x => x.Name).ToList();
         return new Response<List<CurrencyDTO>>()
         {
            Count = await _repository.Count(),
            Content = result.ConvertAll(_mapper.Map<CurrencyDTO>),
            Code = HttpStatusCode.OK,
            Message = result.Count == 0 ? "NoContent" : null
         };
      }

      public async Task<Response<bool>> Import(List<CurrencyDTO> list)
      {
         _logger.LogDebug("Import (list)");
         var result = await _repository.InsertList(list.ConvertAll(_mapper.Map<Currency>));
         _logger.LogTrace($"200. TRUE");
         return new Response<bool>()
         {
            Count = await _repository.Count(),
            Content = true,
            Code = HttpStatusCode.OK,
            Message = null
         };
      }
   }
}
