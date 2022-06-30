using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tyre.WSL.Controllers
{
    [Authorize]
	[ApiController]
    [Route("api/[controller]/[action]")]
    public class ClientiController : ControllerBase
    {
        private readonly IClientiService _clientiService;
        private readonly ILogger<ClientiController> _logger;

        public ClientiController(IClientiService clientiService, ILogger<ClientiController> logger)
        {
            _clientiService = clientiService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _clientiService.GetById(id);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSkipTake(int skip, int take, bool orderByName, string filter)
        {
            try
            {
                var response = await _clientiService.GetAll(skip, take, orderByName, filter);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _clientiService.GetAll();
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ClientiDTO cliente)
        {
            try
            {
                var response = await _clientiService.Add(cliente);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ClientiDTO cliente)
        {
            try
            {
                var response = await _clientiService.Update(cliente);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }
        /*
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var response = await _clientiService.Deactivate(id);
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
