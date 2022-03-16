using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
namespace Tyre.WSL.Controllers
{
	[ApiController]
    [Route("api/[controller]/[action]")]
    public class VeicoliController : ControllerBase
    {
        private readonly IVeicoliService _veicoliService;
        private readonly ILogger<VeicoliController> _logger;

        public VeicoliController(IVeicoliService veicoliService, ILogger<VeicoliController> logger)
        {
            _veicoliService = veicoliService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _veicoliService.GetById(id);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{clienteId}")]
        public async Task<IActionResult> GetByClienteId(int clienteId)
        {
            try
            {
                var response = await _veicoliService.GetByClienteId(clienteId);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSkipTake(int skip, int take, bool orderByTarga, string targa)
        {
            try
            {
                var response = await _veicoliService.GetAll(skip, take, orderByTarga, targa);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForSelectList()
        {
            try
            {
                var response = await _veicoliService.GetAllForSelectList();
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
                var response = await _veicoliService.GetAll();
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] VeicoliDTO veicolo)
        {
            try
            {
                var response = await _veicoliService.Add(veicolo);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] VeicoliDTO veicolo)
        {
            try
            {
                var response = await _veicoliService.Update(veicolo);
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
                var response = await _veicoliService.Deactivate(id);
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
