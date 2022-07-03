using System.Net;
using AutoMapper;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using ManTyres.DAL.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ManTyres.BLL.Services.Implementations
{
	public class CountryService : ICountryService
	{
      private readonly ICountryRepository _repository;
      private readonly IMapper _mapper;
      private readonly ILogger<CountryService> _logger;

      public CountryService(ICountryRepository repository, IMapper mapper, ILogger<CountryService> logger)
      {
         _mapper = mapper;
         _repository = repository;
         _logger = logger;
         _logger.LogDebug("ctor");
      }

		public async Task<Response<List<CountryDTO>>> GetAll()
		{
			_logger.LogDebug("Get (list)");
			var result = await _repository.GetAll();
			_logger.LogTrace($"200. Returned items: {result.Count()}");
			result = result.OrderBy(x => x.Name).ToList();
			return new Response<List<CountryDTO>>()
			{
				Count = await _repository.Count(),
				Content = result.ConvertAll(_mapper.Map<CountryDTO>),
				Code = HttpStatusCode.OK,
				Message = result.Count == 0 ? "NoContent" : null
			};
		}

		public async Task<Response<CountryDTO>> GetByISO(string ISO)
		{
			_logger.LogDebug("Get (list)");
			var result = await _repository.GetByISO(ISO);
			_logger.LogTrace($"200. OK");
			return new Response<CountryDTO>()
			{
				Count = result == null ? 0 : 1,
				Content = result == null ? null : _mapper.Map<CountryDTO>(result),
				Code = result == null ? HttpStatusCode.NotFound : HttpStatusCode.OK,
				Message = result == null ? "NotFound" : null
			};
		}

		public async Task<Response<bool>> Import(List<CountryDTO> list)
		{
			_logger.LogDebug("Get (list)");
			var result = await _repository.InsertList(list.ConvertAll(_mapper.Map<Country>));
			_logger.LogTrace($"200. TRUE");
			return new Response<bool>()
			{
				Count = await _repository.Count(),
				Content = true,
				Code = HttpStatusCode.OK,
				Message = null
			};
		}
		public async Task<Response<bool>> Update()
      {
         _logger.LogDebug("Update (list)");
			string json = File.ReadAllText("countries.json");
			List<NewCountryDTO> countries = JsonConvert.DeserializeObject<List<NewCountryDTO>>(json)!;
			var result = await _repository.UpdateList(countries);
         _logger.LogTrace($"200. TRUE");
         return new Response<bool>()
         {
            Count = countries.Count,
            Content = result,
            Code = result ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
            Message = null
         };
      }
	}
}
