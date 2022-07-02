using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Tyre.WSL.Controllers
{
   [ApiController]
    [Route("api/[controller]/[action]")]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioService _service;
        private readonly ILogger<InventarioController> _logger;

        public InventarioController(IInventarioService service, ILogger<InventarioController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventario(int skip, int take, int sede, int stagione, bool orderByDesc)
        {
            try
            {
                var response = await _service.GetInventario(skip, take, sede, stagione, orderByDesc);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetInventarioBytarga(int skip, int take, int sede, int stagione, bool orderByDesc, string targa)
        {
            try
            {
                var response = await _service.GetInventario(skip, take, sede, stagione, orderByDesc, targa);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetArchivio(int skip, int take, int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc)
        {
            try
            {
                var response = await _service.GetArchivio(skip, take, sede, stagione, inizioOrderByDesc, fineOrderByDesc);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetArchivioByTarga(int skip, int take, int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc, string targa)
        {
            try
            {
                var response = await _service.GetArchivio(skip, take, sede, stagione, inizioOrderByDesc, fineOrderByDesc, targa);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> DelFromArchivio(InventarioDTO item)
        {
            try
            {
                var response = await _service.DelFromArchivio(item);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCestino(int skip, int take, int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc)
        {
            try
            {
                var response = await _service.GetCestino(skip, take, sede, stagione, inizioOrderByDesc, fineOrderByDesc);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCestinoByTarga(int skip, int take, int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc, string targa)
        {
            try
            {
                var response = await _service.GetCestino(skip, take, sede, stagione, inizioOrderByDesc, fineOrderByDesc, targa);
                return StatusCode((int) response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> DelFromCestino(InventarioDTO item)
        {
            try
            {
                var response = await _service.DelFromCestino(item);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> RipristinaFromCestino(InventarioDTO item)
        {
            try
            {
                var response = await _service.RipristinaFromCestino(item);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> RipristinaCestino()
        {
            try
            {
                var response = await _service.RipristinaCestino();
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> SvuotaCestino()
        {
            try
            {
                var response = await _service.SvuotaCestino();
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
