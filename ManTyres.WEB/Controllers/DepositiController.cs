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
    public class DepositiController : ControllerBase
    {
        private readonly IDepositiService _depositiService;
        private readonly ILogger<DepositiController> _logger;

        public DepositiController(IDepositiService depositiService, ILogger<DepositiController> logger)
        {
            _depositiService = depositiService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _depositiService.GetAll();
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DepositiDTO deposito)
        {
            try
            {
                var response = await _depositiService.Add(deposito);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDeposito([FromBody] DepositiDTO deposito)
        {
            try
            {
                var response = await _depositiService.Update(deposito);
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
