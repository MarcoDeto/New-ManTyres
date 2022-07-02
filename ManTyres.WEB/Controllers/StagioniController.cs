using ManTyres.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Tyre.WSL.Controllers
{
   [ApiController]
    [Route("api/[controller]/[action]")]
    public class StagioniController : ControllerBase
    {
        private readonly IStagioniService _stagioniService;
        private readonly ILogger<StagioniController> _logger;

        public StagioniController(IStagioniService stagioniService, ILogger<StagioniController> logger)
        {
            _stagioniService = stagioniService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _stagioniService.GetAllStagioni();
                switch (response.Code)
                {
                    case HttpStatusCode.OK: return Ok(response);

                    case HttpStatusCode.NoContent: return NoContent();

                    default: return NoContent();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }
    }
}