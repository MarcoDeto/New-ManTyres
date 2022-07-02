using System.Net;
using AutoMapper;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using ManTyres.DAL.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;
using Microsoft.Extensions.Logging;

namespace ManTyres.BLL.Services.Implementations
{
	public class LanguageService : ILanguageService
	{
      private readonly ILanguageRepository _languageRepository;
      private readonly IMapper _mapper;
      private readonly ILogger<LanguageService> _logger;

      public LanguageService(ILanguageRepository cityRepository, IMapper mapper, ILogger<LanguageService> logger)
      {
         _mapper = mapper;
         _languageRepository = cityRepository;
         _logger = logger;
         _logger.LogDebug("ctor");
      }

		public async Task<Response<List<LanguageDTO>>> GetAll()
		{
			_logger.LogDebug("Get (list)");
			var result = await _languageRepository.GetAll();
			_logger.LogTrace($"200. Returned items: {result.Count()}");
			return new Response<List<LanguageDTO>>()
			{
				Count = await _languageRepository.Count(),
				Content = result.ConvertAll(_mapper.Map<LanguageDTO>),
				Code = HttpStatusCode.OK,
				Message = result.Count == 0 ? "NoContent" : null
			};
		}

		public async Task<Response<LanguageDTO>> GetByCode(string code)
		{
			_logger.LogDebug("Get");
			var result = await _languageRepository.GetByCode(code);
			_logger.LogTrace($"200. OK");
			return new Response<LanguageDTO>()
			{
				Count = result == null ? 0 : 1,
				Content = result == null ? null : _mapper.Map<LanguageDTO>(result),
				Code = result == null ? HttpStatusCode.NotFound : HttpStatusCode.OK,
				Message = result == null ? "NotFound" : null
			};
		}

		public async Task<Response<bool>> Import(List<LanguageDTO> list)
		{
			_logger.LogDebug("Get (list)");
			var result = await _languageRepository.InsertList(list.ConvertAll(_mapper.Map<Language>));
			_logger.LogTrace($"200. TRUE");
			return new Response<bool>()
			{
				Count = await _languageRepository.Count(),
				Content = true,
				Code = HttpStatusCode.OK,
				Message = null
			};
		}
	}
}
