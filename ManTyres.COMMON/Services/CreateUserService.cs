//using Azure.Identity;
using ManTyres.COMMON.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ManTyres.COMMON.Services
{
	public class CreateUserService : ICreateUserService
	{
		private readonly IConfiguration _configuration;
		//private readonly ISmtpService _smtpService;

		public CreateUserService(IConfiguration configuration/*, ISmtpService smtpService*/)
		{
			_configuration = configuration;
			//_smtpService = smtpService;
		}

		public async Task<User> CreateUser(User user)
		{
			DelegateAuthenticationProvider authProvider = await _getGraphConfiguration();

			// Istanza del graphClient
			var graphClient = new GraphServiceClient(authProvider);

			try
			{
				user.PasswordProfile = new PasswordProfile()
				{
					ForceChangePasswordNextSignIn = true,
					Password = _generateRandomPassword()
				};

				User graphResult = await graphClient.Users.Request().AddAsync(user);
				if (graphResult == null)
					throw new InvalidOperationException("An error occurred while adding user to Active Directory. Please try again.");

				/*BackOfficeMailDTO mail = new()
				{
					emailSender = _configuration.GetSection("EmailVSG:Sender").Value,
					recipientEmail = user.Mail,
					emailBody = "",
					emailSubject = ""
				};
				bool emailResult = false;
				emailResult = await _smtpService.ForceChangePasswordMail(mail, user.PasswordProfile.Password, user.GivenName);
				if (emailResult)
					return graphResult;
				else
					throw new InvalidOperationException("An error occurred while sending email to newly created user.");*/
				return null;
			}
			catch (Exception ex)
			{
				throw new Exception("Users.Errors.AlreadyExist");
			}
		}

		private async Task<DelegateAuthenticationProvider> _getGraphConfiguration()
		{
			// L'Application ID Url + /.default
			var scopes = new[] { "https://dltvsg.onmicrosoft.com/dltvsg/.default" };

			// ClientId e ClientSecret dell'AppRegistration (TeqLinkPortal)
			var clientId = "172ca3da-66dc-4964-9f3f-ae969159897b";
			var clientSecret = "XHQLINdY3AKUKj_MW4G_z-njv.Z3w1Vh~j";

			// Configurazione default
			/*var options = new TokenCredentialOptions
			{
				AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
			};
			*/
			// Variabile in cui salvare il token
			var clientToken = "JWT_TOKEN_TO_EXCHANGE";

			// Istanza del client per la richiesta di acquisizione del token
			var app = ConfidentialClientApplicationBuilder.Create(clientId)
				.WithClientSecret(clientSecret)
				.WithAuthority(new Uri("https://login.microsoftonline.com/dltvsg.onmicrosoft.com"))
				.Build();

			// Scope di Microsoft Graph
			IEnumerable<string> b2cscopes = new List<string> { "https://graph.microsoft.com/.default" };

			// Richiesta del token
			var tokenResult = await app.AcquireTokenForClient(b2cscopes).ExecuteAsync();

			// Costruzione dell'authProvider per il graphClient
			var authProvider = new DelegateAuthenticationProvider(async (request) =>
			{
				string tokenUrl = "https://login.microsoftonline.com/dltvsg.onmicrosoft.com/oauth/v2.0/token";
				var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenUrl);

				tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					["grant_type"] = "client_credentials",
					["client_id"] = "172ca3da-66dc-4964-9f3f-ae969159897b", // AppRegistration applicationId
					["client_secret"] = "XHQLINdY3AKUKj_MW4G_z-njv.Z3w1Vh~j", // secret dell'AppRegistration
					["scope"] = "https://graph.microsoft.com/" // resource
				});

				// oboToken = Access Token dall'AcquireTokenForClient
				clientToken = tokenResult.AccessToken;

				// Assertion dell'access token
				var assertion = new UserAssertion(clientToken);

				// Aggiunta del token agli headers della richiesta
				request.Headers.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken);
			});

			return authProvider;
		}

		public string _generateRandomPassword()
		{
			/*PasswordOptions opts = new PasswordOptions()
			{
				RequiredLength = 12,
				RequiredUniqueChars = 4,
				RequireDigit = true,
				RequireLowercase = true,
				RequireNonAlphanumeric = true,
				RequireUppercase = true
			};*/

			string[] randomChars = new[] {
					"ABCDEFGHJKLMNOPQRSTUVWXYZ",
					"abcdefghijkmnopqrstuvwxyz",
					"0123456789",
					"!@$?_-"
				};
			Random rand = new Random();
			List<char> chars = new List<char>();
			/*
			if (opts.RequireUppercase)
				chars.Insert(rand.Next(0, chars.Count),
					 randomChars[0][rand.Next(0, randomChars[0].Length)]);

			if (opts.RequireLowercase)
				chars.Insert(rand.Next(0, chars.Count),
					 randomChars[1][rand.Next(0, randomChars[1].Length)]);

			if (opts.RequireDigit)
				chars.Insert(rand.Next(0, chars.Count),
					 randomChars[2][rand.Next(0, randomChars[2].Length)]);

			if (opts.RequireNonAlphanumeric)
				chars.Insert(rand.Next(0, chars.Count),
					 randomChars[3][rand.Next(0, randomChars[3].Length)]);

			for (int i = chars.Count; i < opts.RequiredLength
				 || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
			{
				string rcs = randomChars[rand.Next(0, randomChars.Length)];
				chars.Insert(rand.Next(0, chars.Count),
					 rcs[rand.Next(0, rcs.Length)]);
			}*/

			return new string(chars.ToArray());
		}
	}
}
