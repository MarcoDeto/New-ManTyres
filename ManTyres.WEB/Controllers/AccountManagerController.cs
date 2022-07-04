using ManTyres.BLL.Services;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tyre.WSL.Controllers
{
   [ApiController]
   [Route("api/[controller]/[action]")]
   public class AccountManagerController : ControllerBase
   {
      private readonly IOptions<JwtSettings> _appSettings;
      private readonly IUserService _userService;
      private readonly ILogger<AccountManagerController> _logger;

      public AccountManagerController(IOptions<JwtSettings> appSettings, IUserService userService, ILogger<AccountManagerController> logger)
      {
         _appSettings = appSettings;
         _userService = userService;
         _logger = logger;
      }

      [HttpPost]
      public async Task<IActionResult> Login([FromBody] LoginDTO model)
      {
         try
         {
            var user = await _userService.CheckLogin(model);
            if (user.Code != HttpStatusCode.OK || user.Content == null)
               return BadRequest(user);
            else if (user.Content != null && user.Content.IsDeleted)
               return BadRequest(new Response<object> { Content = null, Code = HttpStatusCode.Unauthorized, Count = 0, Message = "ERROR_REMOVED" });

            IdentityOptions _options = new IdentityOptions();

            var key = Encoding.UTF8.GetBytes(_appSettings.Value.JWT_Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
               Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(type: "UserID", value: user!.Content!.Id.ToString()),
                        new Claim(type: "UserName", value: user!.Content!.UserName!),
                        new Claim(type: "Role", value: user!.Content!.Role.ToString())
                }),
               Expires = DateTime.Now.AddHours(int.Parse(_appSettings.Value.JWT_Expire)),
               SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
				var expiration = tokenDescriptor.Expires;
            var token = tokenHandler.WriteToken(securityToken);

            _logger.LogInformation($"{user!.Content!.UserName!} Login at {DateTime.Now}");

            return Ok(new Response<object> { 
					Content = new { 
						token = token, 
						expiration = expiration, 
						user = user.Content
					},
					Code = HttpStatusCode.OK, 
					Count = 1, 
					Message = null
				});
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

		[HttpPost]
		public async Task<IActionResult> CheckCurrentPassword([FromBody] UserPasswordDTO model)
		{
			try
			{
				var response = await _userService.CheckCurrentPassword(model);
				return StatusCode((int)response.Code, response);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Errore");
				return StatusCode(500, e.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword([FromBody] UserPasswordDTO model)
		{
			try
			{
				var response = await _userService.ChangePassword(model);
				return StatusCode((int)response.Code, response);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Errore");
				return StatusCode(500, e.Message);
			}
		}

      /*[HttpGet]
		[Authorize(Roles = "admin, user")]
		public async Task<ActionResult> Profile(string username)
		{
			try
			{
				var response = await _userService.GetUserbyUsername(username);
				return StatusCode((int)response.Code, response);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Errore");
				return StatusCode(500, e.Message);
			}
		}

		[HttpPut]
		public async Task<ActionResult> ResetPassword([FromBody] UserPasswordDTO model)
		{
			try
			{
				var response = await _userService.ResetPassword(model);
				return StatusCode((int)response.Code, response);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Errore");
				return StatusCode(500, e.Message);
			}
		}
		/*
		[HttpGet("email")]
		[AllowAnonymous]
		public async Task<ActionResult> MissedPassword(string email)
		{
			 try
			 {
				  var response = await _userService.MissedPassword(email);
				  return StatusCode((int)response.Code, response);
			 }
			 catch (Exception e)
			 {
				  _logger.LogError(e, "Errore");
				  return StatusCode(500, e.Message);
			 }
		}*/
   }
}
