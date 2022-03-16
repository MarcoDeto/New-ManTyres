using ManTyres.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Tyre.WSL.Controllers
{
	[ApiController]
    [Route("api/[controller]/[action]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Pneumatici()
        {
            try
            {
                var response = _dashboardService.TotalePneumatici();
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public IActionResult Veicoli()
        {
            try
            {
                var response = _dashboardService.TotaleVeicoli();
                return StatusCode((int) response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public IActionResult Clienti()
        {
            try
            {
                var response = _dashboardService.TotaleClienti();
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public IActionResult Utenti()
        {
            try
            {
                var response = _dashboardService.TotaleUtenti();
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChartGiornalieroPneumatici(int month, string ci)
        {
            try
            {
                var response = await _dashboardService.ChartQuantitàPneumatici(month, ci);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChartGlobalePneumatici(int month, string ci)
        {
            try
            {
                var response = await _dashboardService.ChartGlobalePneumatici(month, ci);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChartGiornalieroVeicoli(int month, string ci)
        {
            try
            {
                var response = await _dashboardService.ChartQuantitàVeicoli(month, ci);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChartGlobaleVeicoli(int month, string ci)
        {
            try
            {
                var response = await _dashboardService.ChartGlobaleVeicoli(month, ci);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChartGiornalieroClienti(int month, string ci)
        {
            try
            {
                var response = await _dashboardService.ChartQuantitàClienti(month, ci);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChartGlobaleClienti(int month, string ci)
        {
            try
            {
                var response = await _dashboardService.ChartGlobaleClienti(month, ci);
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
