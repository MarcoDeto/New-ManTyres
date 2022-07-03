using ManTyres.BLL.Services;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Tyre.WSL.Controllers
{
   [ApiController]
   [AllowAnonymous]
   [Route("api/[controller]/[action]")]
   public class FirstConnectionController : ControllerBase
   {
      //private readonly IPneumaticiService _pneumaticiService;
      private readonly ILanguageService _langService;
      private readonly ICurrencyService _currencyService;
      private readonly ICountryService _countryService;

      private readonly ILogger<FirstConnectionController> _logger;
      public FirstConnectionController(
          //IFirstConnectionService service,
          ILanguageService langService,
          ICurrencyService currencyService,
          ICountryService countryService,
          //IPneumaticiService pneumaticiService,
          ILogger<FirstConnectionController> logger)
      {
         _currencyService = currencyService;
         _countryService = countryService;
         _langService = langService;
         //_pneumaticiService = pneumaticiService;
         _logger = logger;
      }

      [HttpGet]
      public async Task<IActionResult> GetCurrLangs(string country_code)
      {
         try
         {
            List<LanguageDTO> languages = new List<LanguageDTO>();
				List<LanguageDTO> languages_suggested = new List<LanguageDTO>();
            List<CurrencyDTO> currencies = new List<CurrencyDTO>();
            Response<List<LanguageDTO>> resLang = await _langService.GetAll();
            if (resLang.Content != null && resLang.Content.Count() > 0)
            {
               var firstLanguage = resLang.Content.FirstOrDefault(
                  x => x.Code != null && x.Code.EndsWith(country_code)
               );
					var enGBLanguage = resLang.Content.FirstOrDefault(
                  x => x.Code != null && x.Code == "en-GB"
					);
					var enUSLanguage = resLang.Content.FirstOrDefault(
                  x => x.Code != null && x.Code == "en-US"
					);
               if (firstLanguage != null)
               {
                  languages.Add(firstLanguage);
						languages_suggested.Add(firstLanguage);
                  resLang.Content.Remove(firstLanguage);
               }
               languages.AddRange(resLang.Content);
					if (enGBLanguage != null) { languages_suggested.Add(enGBLanguage); }
					if (enUSLanguage != null) { languages_suggested.Add(enUSLanguage); }
            }
            Response<CountryDTO> country = await _countryService.GetByISO(country_code);
            Response<List<CurrencyDTO>> resCurr = await _currencyService.GetAll();
            if (resCurr.Content != null && resCurr.Content.Count() > 0)
            {
               if (country.Content != null && country.Content.CurrencyCode != null)
               {
                  var firstCurrency = resCurr.Content.FirstOrDefault(
                     x => x.Code != null &&
                     x.Code.EndsWith(country.Content.CurrencyCode)
                  );
                  if (firstCurrency != null)
                  {
                     currencies.Add(firstCurrency);
                     resCurr.Content.Remove(firstCurrency);
                  }
               }
               currencies.AddRange(resCurr.Content);
            }
            return StatusCode(
               (int)HttpStatusCode.OK,
					new
					{
						languages = languages,
						currencies = currencies,
						country = country.Content,
						languages_suggested = languages_suggested
					}
            );
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      /*[HttpGet]
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
      }*/
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
      public string? email { get; set; }
   }
   public class Oggetto
   {
      public List<Subscriber>? subscribers { get; set; }
   }
}
