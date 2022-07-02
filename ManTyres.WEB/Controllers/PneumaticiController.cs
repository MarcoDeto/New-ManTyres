using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Tyre.WSL.Controllers
{
   [ApiController]
    [Route("api/[controller]/[action]")]
    public class PneumaticiController : ControllerBase
    {
        private readonly IPneumaticiService _pneumaticiService;
        private readonly ILogger<PneumaticiController> _logger;
        public PneumaticiController(IPneumaticiService pneumaticiService, ILogger<PneumaticiController> logger)
        {
            _pneumaticiService = pneumaticiService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetLast()
        {
            try
            {
                var response = await _pneumaticiService.GetLast2();
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByTarga(string targa)
        {
            try
            {
                var response = await _pneumaticiService.GetByTarga(targa);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InventarioDTO pneumatici)
        {
            try
            {
                var response = await _pneumaticiService.Add(pneumatici);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] InventarioDTO pneumatici)
        {
            try
            {
                var response = await _pneumaticiService.Update(pneumatici);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> End([FromBody] InventarioDTO pneumatici)
        {
            try
            {
                var response = await _pneumaticiService.FineDeposito(pneumatici);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Start([FromBody] InventarioDTO pneumatici)
        {
            try
            {
                var response = await _pneumaticiService.InizioDeposito(pneumatici);
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
