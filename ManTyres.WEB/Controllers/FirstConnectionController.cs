using ManTyres.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tyre.WSL.Controllers
{
	[ApiController]
	[AllowAnonymous]
	[Route("api/[controller]/[action]")]
	public class FirstConnectionController : ControllerBase
	{
		private readonly IPneumaticiService _pneumaticiService;
		private readonly ILogger<FirstConnectionController> _logger;
		public FirstConnectionController(
			 IFirstConnectionService service,
			 IPneumaticiService pneumaticiService,
			 ILogger<FirstConnectionController> logger)
		{
			_pneumaticiService = pneumaticiService;
			_logger = logger;
		}


		[HttpGet]
		public async Task<IActionResult> GenerateData(string password)
		{
			try
			{
				if (password != "PasswordSegretissima12345!")
					return StatusCode(403, "Password errata!");
				var response = await _pneumaticiService.GenerateData();
				return StatusCode((int)response.Code, response);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Errore");
				return StatusCode(500, e.Message);
			}
		}
		[HttpGet]
		public async Task<IActionResult> subscribeEmail(string email)
		{
			try
			{
				HttpClient client = new HttpClient();
				Subscriber subscribe = new Subscriber()
				{
					email = email
				};
				List<Subscriber> subscribers = new List<Subscriber>();
				subscribers.Add(subscribe);
				var user = new Oggetto { subscribers = subscribers };
				// Serialize our concrete class into a JSON String
				var stringPayload = JsonConvert.SerializeObject(user);
				// Wrap our JSON inside a StringContent which then can be used by the HttpClient class
				var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
				httpContent.Headers.Add("X-MailerLite-ApiKey", "860914397107729f2a1c066b44f310e2");
				// Do the actual request and await the response
				var httpResponse = await client.PostAsync("https://api.mailerlite.com/api/v2/groups/108708065/subscribers/import", httpContent);
				return StatusCode(((int)httpResponse.StatusCode));
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Errore");
				return StatusCode(500, e.Message);
			}
		}
		private static async Task<HttpStatusCode> AddSubscriber(string email)
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Add("X-MailerLite-ApiKey", "860914397107729f2a1c066b44f310e2");
			Subscriber subscribe = new Subscriber()
			{
				email = email
			};
			List<Subscriber> subscribers = new List<Subscriber>();
			subscribers.Add(subscribe);
			var user = new Oggetto { subscribers = subscribers };
			// Serialize our concrete class into a JSON String
			var stringPayload = JsonConvert.SerializeObject(user);
			// Wrap our JSON inside a StringContent which then can be used by the HttpClient class
			var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
			// Do the actual request and await the response
			var httpResponse = await client.PutAsync("http://localhost:15672/api/users/I4IOTGateway", httpContent);

			return httpResponse.StatusCode;
		}
	}

	public class Subscriber
	{
		public string email { get; set; }
	}
	public class Oggetto
	{
		public List<Subscriber> subscribers { get; set; }
	}
}
