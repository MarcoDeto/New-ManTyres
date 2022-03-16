using System.Net;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tyre.WSL.Controllers
{
   [ApiController]
   [Route("api/[controller]/[action]")]
   public class UserManagerController : ControllerBase
   {
      private readonly IUserService _userService;
      private readonly ILogger<UserManagerController> _logger;
      public UserManagerController(IUserService userService, ILogger<UserManagerController> logger)
      {
         _userService = userService;
         _logger = logger;
      }

      [HttpGet]
      public async Task<ActionResult> GetAll(int skip, int take)
      {
         try
         {
            var response = await _userService.Get(skip, take);
            return StatusCode((int)response.Code, response);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      [HttpGet("{id}")]
      public async Task<ActionResult> GetById(string id)
      {
         try
         {
            var response = await _userService.Get(id);
            return StatusCode((int)response.Code, response);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      /*[HttpGet]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult> GetRolesUser(string userId)
		{
			 try
			 {
				  var response = await _userService.GetRolesUser(userId);
				  return StatusCode((int)response.Code, response);
			 }
			 catch (Exception e)
			 {
				  _logger.LogError(e, "Errore");
				  return StatusCode(500, e.Message);
			 }
		}*/

      [HttpPost]
      public async Task<ActionResult> Create([FromBody] UserDTO request)
      {
         try
         {
            var email = new System.Net.Mail.MailAddress(request.Email!);
         }
         catch (FormatException)
         {
            return StatusCode((int)HttpStatusCode.UnprocessableEntity, "ERROR_INVALIDEMAIL");
         }

         try
         {
            var response = await _userService.CreateAccount(request);
            return StatusCode((int)response.Code, response);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      /*[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail(string userId, string token)
		{
			 try
			 {
				  var response = await _userService.ConfirmEmail(userId, token);
				  return StatusCode((int)response.Code, response);
			 }
			 catch (Exception e)
			 {
				  _logger.LogError(e, "Errore");
				  return StatusCode(500, e.Message);
			 }
		}*/

      [HttpPut]
      public async Task<ActionResult> Update([FromBody] UserDTO user)
      {
         try
         {
            var response = await _userService.Put(user);
            return StatusCode((int)response.Code, response);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }

      [HttpDelete("{id}")]
      [Authorize(Roles = "admin")]
      public async Task<ActionResult> Delete(string id)
      {
         try
         {
            var response = await _userService.Deactive(id);
            return StatusCode((int)response.Code, response);
         }
         catch (Exception e)
         {
            _logger.LogError(e, "Errore");
            return StatusCode(500, e.Message);
         }
      }
   }
}
