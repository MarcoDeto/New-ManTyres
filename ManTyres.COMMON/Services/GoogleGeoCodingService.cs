using ManTyres.COMMON.Interfaces;
using ManTyres.COMMON.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManTyres.COMMON.Services
{
	public class GoogleGeoCodingService : IGoogleGeoCodingService
	{
		private readonly ILogger<GoogleGeoCodingService> _logger;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IOptions<GoogleOptions> _configuration;
		private readonly ErrorsManager _errorsManager;

		public GoogleGeoCodingService(ILogger<GoogleGeoCodingService> logger, IHttpClientFactory httpClientFactory, IOptions<GoogleOptions> configuration, ErrorsManager errorsManager)
		{
			_logger = logger;
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
			_errorsManager = errorsManager;
		}

		public virtual async Task<DataBridge<GoogleGeoCodingResponse>> GeoCodeAsync(string address)
		{
			string APIKEY = _configuration.Value.ApiKey;
			string geocodeBaseUrl = _configuration.Value.GeoCodingBaseUrl;

			try
			{
				if (string.IsNullOrWhiteSpace(APIKEY))
					return _errorsManager.LogAndGetDataBridge<GoogleGeoCodingService, GoogleGeoCodingResponse>(_logger, new ArgumentNullException(APIKEY));
				else if (string.IsNullOrWhiteSpace(address))
					return _errorsManager.LogAndGetDataBridge<GoogleGeoCodingService, GoogleGeoCodingResponse>(_logger, new ArgumentNullException(address));
				else if (string.IsNullOrWhiteSpace(geocodeBaseUrl))
					return _errorsManager.LogAndGetDataBridge<GoogleGeoCodingService, GoogleGeoCodingResponse>(_logger, new ArgumentNullException(geocodeBaseUrl));

				var builder = new StringBuilder();

				builder.Append(geocodeBaseUrl);
				builder.Append("address=");
				builder.Append(Uri.EscapeDataString(address));
				builder.Append("&key=");
				builder.Append(APIKEY);

				var response = await _httpClientFactory.CreateClient().GetAsync(builder.ToString());
				var content = await response.Content.ReadAsStringAsync();
				var responseBody = JsonSerializer.Deserialize<GoogleGeoCodingResponse>(content, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				return new DataBridge<GoogleGeoCodingResponse> { Data = responseBody, StatusCode = DataBridgeStatusCode.Ok };
			}
			catch (Exception ex)
			{
				return _errorsManager.LogAndGetDataBridge<GoogleGeoCodingService, GoogleGeoCodingResponse>(_logger, ex);
			}
		}
	}
}
