using System.Net;
using AutoMapper;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.DAL.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;
using Microsoft.Extensions.Logging;

namespace ManTyres.BLL.Services.Implementations
{
	public class CityService : ICityService
	{
      private readonly ICityRepository _cityRepository;
      private readonly IMapper _mapper;
      private readonly ILogger<CityService> _logger;

      public CityService(ICityRepository cityRepository, IMapper mapper, ILogger<CityService> logger)
      {
         _mapper = mapper;
         _cityRepository = cityRepository;
         _logger = logger;
         _logger.LogDebug("ctor");
      }

		public async Task<Response<long>> CountByISO(string ISO)
		{
			_logger.LogDebug("Get (list)");
			var result = await _cityRepository.CountByISO(ISO);
			_logger.LogTrace($"200. Returned items: {result}");
			return new Response<long>()
			{
				Count = result,
				Content = result,
				Code = HttpStatusCode.OK,
				Message = result == 0 ? "NoContent" : null
			};
		}

		public async Task<Response<List<CityDTO>>> GetAll()
		{
			_logger.LogDebug("Get (list)");
			var result = await _cityRepository.GetAll();
			_logger.LogTrace($"200. Returned items: {result.Count()}");
			return new Response<List<CityDTO>>()
			{
				Count = await _cityRepository.Count(),
				Content = result.ConvertAll(_mapper.Map<CityDTO>),
				Code = HttpStatusCode.OK,
				Message = result.Count == 0 ? "NoContent" : null
			};
		}

		public async Task<Response<List<CityDTO>>> GetByISO(string ISO)
		{
			_logger.LogDebug("Get (list)");
			var result = await _cityRepository.GetByISO(ISO);
			_logger.LogTrace($"200. Returned items: {result.Count()}");
			result.OrderBy(x => x.Name);
			return new Response<List<CityDTO>>()
			{
				Count = result.Count,
				Content = result.ConvertAll(_mapper.Map<CityDTO>),
				Code = HttpStatusCode.OK,
				Message = result.Count == 0 ? "NoContent" : null
			};
		}

		public async Task<Response<bool>> Import(List<CityDTO> list)
		{
			_logger.LogDebug("Get (list)");
			var result = await _cityRepository.InsertList(list.ConvertAll(_mapper.Map<City>));
			_logger.LogTrace($"200. TRUE");
			return new Response<bool>()
			{
				Count = await _cityRepository.Count(),
				Content = true,
				Code = HttpStatusCode.OK,
				Message = null
			};
		}
	}
}
