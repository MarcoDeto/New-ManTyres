using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tyre.WSL.Controllers
{
   [ApiController]
    [Route("api/[controller]/[action]")]
    public class SediController : ControllerBase
    {
        private readonly ISediService _sediService;
        private readonly ILogger<SediController> _logger;

        public SediController(ISediService sediService, ILogger<SediController> logger)
        {
            _sediService = sediService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _sediService.GetAllSedi();
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _sediService.GetById(id);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SediDTO sede)
        {
            try
            {
                var response = await _sediService.AddSede(sede);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SediDTO sede)
        {
            try
            {
                var response = await _sediService.UpdateSede(sede);
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
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var response = await _sediService.DeactivateSede(id);
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
