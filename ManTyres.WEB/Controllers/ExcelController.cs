using ManTyres.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Tyre.WSL.Controllers
{
   [ApiController]
    [Route("api/[controller]/[action]")]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelService _service;
        private readonly ILogger<ExcelController> _logger;

        public ExcelController(IExcelService service, ILogger<ExcelController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult TracciatoClienti()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Tracciato clienti.xlsx";
            try
            {
                var response = _service.TracciatoClienti();
                return File(response.Content, contentType, fileName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public IActionResult TracciatoVeicoli()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Tracciato veicoli.xlsx";
            try
            {
                var response = _service.TracciatoVeicoli();
                return File(response.Content, contentType, fileName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public IActionResult TracciatoPneumatici()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Tracciato completo.xlsx";
            try
            {
                var response = _service.TracciatoAll();
                return File(response.Content, contentType, fileName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        /*[HttpPost]
        public async Task<IActionResult> ImportClienti(IFormFile file)
        {

            if (file == null) { return BadRequest(new Response<Exception>(null, 0, HttpStatusCode.BadRequest, "File obbligatorio")); }
            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    var response = await _service.ImportClienti(stream);
                    switch (response.Code)
                    {
                        case HttpStatusCode.OK: return Ok(response);

                        case HttpStatusCode.BadRequest: return BadRequest(response);

                        default: return NoContent();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportVeicoli(IFormFile file)
        {
            if (file == null) { return BadRequest(new Response<Exception>(null, 0, HttpStatusCode.BadRequest, "File obbligatorio")); }
            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    var response = await _service.ImportVeicoli(stream);
                    switch (response.Code)
                    {
                        case HttpStatusCode.OK: return Ok(response);

                        case HttpStatusCode.BadRequest: return BadRequest(response);

                        default: return NoContent();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportAll(IFormFile file, string ci, int sedeId, string userId)
        {
            if (file == null) { return BadRequest(new Response<Exception>(null, 0, HttpStatusCode.BadRequest, "File obbligatorio")); }
            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    var response = await _service.ImportAll(stream, sedeId, userId, ci);
                    switch (response.Code)
                    {
                        case HttpStatusCode.OK: return Ok(response);

                        case HttpStatusCode.BadRequest: return BadRequest(response);

                        default: return NoContent();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }*/


        /*[HttpGet]
        public async Task<IActionResult> ExportClienti()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Clienti.xlsx";
            try
            {
                var response = await _service.ExportClienti();
                return File(response.Content, contentType, fileName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportVeicoli()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Veicoli.xlsx";
            try
            {
                var response = await _service.ExportVeicoli();
                return File(response.Content, contentType, fileName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportPneumatici(int sede, int stagione, bool orderByDesc)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Storico.xlsx";
            try
            {
                var response = await _service.ExportPneumatici(sede, stagione, orderByDesc);
                return File(response.Content, contentType, fileName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportInventario(int sede, int stagione, bool orderByDesc)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Inventario.xlsx";
            try
            {
                var response = await _service.ExportInventario(sede, stagione, orderByDesc);
                return File(response.Content, contentType, fileName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportArchivio(int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Archivio.xlsx";
            try
            {
                var response = await _service.ExportArchivio(sede, stagione, inizioOrderByDesc, fineOrderByDesc);
                return File(response.Content, contentType, fileName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportCestino(int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Cestino.xlsx";
            try
            {
                var response = await _service.ExportCestino(sede, stagione, inizioOrderByDesc, fineOrderByDesc);
                return File(response.Content, contentType, fileName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }*/
    }
}

