using AutoMapper;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.Utils;
using ManTyres.DAL.Infrastructure.MongoDB.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ManTyres.BLL.Services.Implementations
{
	public class BaseService<T, U, V> : IBaseService<T, U, V>
		 where T : class
		 where V : class
		 where U : IBaseRepository<V>
	{
		private readonly U _repository;
		private readonly IMapper _mapper;
		private readonly ILogger<BaseService<T, U, V>> _logger;

		public BaseService(U repository, IMapper mapper, ILogger<BaseService<T, U, V>> logger)
		{
			_logger = logger;
			_repository = repository;
			_mapper = mapper;
			_logger.LogDebug("ctor");
		}

		public async Task<Response<List<T>>> Get(int skip, int limit)
		{
			_logger.LogDebug("Get (list)");
			var result = await _repository.GetAll(skip, limit);
			_logger.LogTrace($"200. Returned items: {result.Count()}");
			return new Response<List<T>>()
			{
				Count = await _repository.Count(),
				Content = result.ConvertAll(_mapper.Map<T>),
				Code = HttpStatusCode.OK,
				Message = result.Count == 0 ? "NoContent" : null
			};
		}

		public async Task<Response<T>> Get(string id)
		{
			_logger.LogDebug("Get (item)");
			var result = await _repository.Get(id);
			if (result == null)
				_logger.LogTrace("404. Not found");

			_logger.LogTrace($"200. Returned item", result);
			return new Response<T>()
			{
				Count = result == null ? 0 : 1,
				Content = _mapper.Map<T>(result),
				Code = result == null ? HttpStatusCode.NotFound : HttpStatusCode.OK,
				Message = result == null ? "NotFound" : null
			};
		}

		/*
		public List<T> GetFiltered(List<V> filteredList)
		{
			_logger.LogDebug(LoggerHelper.GetActualMethodName());
			var result = new List<T>();
			if (filteredList.Count() != 0)
				filteredList.ForEach(g => result.Add(_mapper.Map<T>(g)));
			_logger.LogTrace($"200. Returned items: {result.Count()}");
			return result;
		}*/

		public async Task<Response<bool>> PostReturnBool(T request)
		{
			_logger.LogDebug(LoggerHelper.GetActualMethodName());
			_logger.LogTrace("Request: ", request);

			var test = _mapper.Map<V>(request);

			var result = await _repository.Add(_mapper.Map<V>(request));
			if (result == null)
				_logger.LogTrace("422. Unprocessable entity");

			_logger.LogTrace($"200. Returned successful", result);
			return new Response<bool>()
			{
				Count = result == null ? 0 : 1,
				Content = result == null ? false : true,
				Code = result == null ? HttpStatusCode.UnprocessableEntity : HttpStatusCode.OK,
				Message = result == null ? "UnprocessableEntity" : "SuccessfullyAdded"
			};
		}

		public async Task<Response<T>> PostReturnObj(T request)
		{
			_logger.LogDebug(LoggerHelper.GetActualMethodName());
			_logger.LogTrace("Request: ", request);

			var result = await _repository.Add(_mapper.Map<V>(request));
			if (result == null)
				_logger.LogTrace("422. Unprocessable entity");

			_logger.LogTrace($"200. Returned successful", result);
			return new Response<T>()
			{
				Count = result == null ? 0 : 1,
				Content = _mapper.Map<T>(result),
				Code = result == null ? HttpStatusCode.UnprocessableEntity : HttpStatusCode.OK,
				Message = result == null ? "UnprocessableEntity" : "SuccessfullyAdded"
			};
		}

		public async Task<Response<T>> Put(T request)
		{
			_logger.LogDebug(LoggerHelper.GetActualMethodName());
			_logger.LogTrace("Request: ", request);

			var result = await _repository.Update(_mapper.Map<V>(request));
			if (result == null)
				_logger.LogTrace("422. Unprocessable entity");

			_logger.LogTrace($"200. Returned successful", result);
			return new Response<T>()
			{
				Count = result == null ? 0 : 1,
				Content = _mapper.Map<T>(result),
				Code = result == null ? HttpStatusCode.UnprocessableEntity : HttpStatusCode.OK,
				Message = result == null ? "UnprocessableEntity" : "SuccessfullyUpdated"
			};
		}

		public async Task<Response<bool>> Deactive(string id)
		{
			_logger.LogDebug(LoggerHelper.GetActualMethodName());
			_logger.LogTrace("Entity id: ", id);

			var result = await _repository.Deactive(id);
			if (result == false)
				_logger.LogTrace("404. Not found");

			_logger.LogTrace($"200. Returned successful", result);
			return new Response<bool>()
			{
				Count = result == false ? 0 : 1,
				Content = result,
				Code = result == false ? HttpStatusCode.NotFound : HttpStatusCode.OK,
				Message = result == false ? "NotFound" : "SuccessfullyDeleted"
			};
		}

		public async Task<Response<bool>> Delete(string id)
		{
			_logger.LogDebug(LoggerHelper.GetActualMethodName());
			_logger.LogTrace("Entity id: ", id);

			var result = await _repository.Delete(id);
			if (result == false)
				_logger.LogTrace("404. Not found");

			_logger.LogTrace($"200. Returned successful", result);
			return new Response<bool>()
			{
				Count = result == false ? 0 : 1,
				Content = result,
				Code = result == false ? HttpStatusCode.NotFound : HttpStatusCode.OK,
				Message = result == false ? "NotFound" : "SuccessfullyDeleted"
			};
		}
	}
}
