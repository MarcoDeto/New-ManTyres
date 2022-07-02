using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using ManTyres.DAL.SQLServer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Tyre.WSL.Controllers
{
   [ApiController]
    [Route("api/[controller]/[action]")]
    public class RolesManagerController : ControllerBase
    {
        private readonly IRoleService _roleService; 
        private readonly ILogger<RolesManagerController> _logger;
        public RolesManagerController(IRoleService roleService, ILogger<RolesManagerController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var response = await _roleService.GetAll();
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllSkipTake(int skip, int take)
        {
            try
            {
                var response = await _roleService.GetAll(skip, take);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetDisponibili(string userId)
        {
            try
            {
                var response = await _roleService.GetRolesDisponibiliByUserId(userId);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                var response = await _roleService.GetById(id);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ApplicationRole role)
        {
            try
            {
                var response = await _roleService.Create(role.Name);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RoleDTO role)
        {
            try
            {
                var response = await _roleService.Update(role);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var response = await _roleService.Delete(id);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Deactive(string id)
        {
            try
            {
                var response = await _roleService.Deactive(id);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetRolesUser(string userId)
        {
            try
            {
                var response = await _roleService.GetRolesUser(userId);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddRoleUser([FromBody] UserRoleDTO user)
        {
            try
            {
                var response = await _roleService.AddRoleUser(user);
                return StatusCode((int)response.Code, response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Errore");
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> DelRoleUser([FromBody] UserRoleDTO user)
        {
            try
            {
                var response = await _roleService.DelRoleUser(user);
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
