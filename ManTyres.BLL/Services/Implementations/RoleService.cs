using AutoMapper;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using ManTyres.DAL.SQLServer;
using ManTyres.DAL.SQLServer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Implementations
{
	public class RoleService : IRoleService
	{
		private readonly IMapper _mapper;
		private readonly TyreDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		public RoleService(IMapper mapper, TyreDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
		{
			_mapper = mapper;
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		#region METHODS
		public async Task<Response<List<RoleDTO>>> GetAll()
		{
			var result = await _roleManager.Roles.ToListAsync();
			return Response(result, result.Count);
		}
		public async Task<Response<List<RoleDTO>>> GetAll(int skip, int take)
		{
			var result = await _context.Roles.Skip(skip).Take(take).Where(x => x.IsDeleted == false)
				 .OrderBy(x => x.Name).ToListAsync();
			var count = _context.Roles.Where(x => x.IsDeleted == false).Count();
			return Response(result, count);
		}
		public async Task<Response<List<RoleDTO>>> GetRolesDisponibiliByUserId(string userId)
		{
			var _user = await _userManager.FindByIdAsync(userId);
			if (_user == null)
				return new Response<List<RoleDTO>>(null, 0, HttpStatusCode.NotFound, $"Nessun utente con Id = {userId}");
			var result = await _context.Roles.OrderBy(x => x.Name).ToListAsync();
			var rolesUser = await _userManager.GetRolesAsync(_user);
			foreach (var item in rolesUser)
			{
				var toRemove = await _roleManager.FindByNameAsync(item);
				result.Remove(toRemove);
			}
			return Response(result, result.Count);
		}
		public async Task<Response<RoleDTO>> GetById(string id)
		{
			if ((await CheckId(id)).Content == false)
				return new Response<RoleDTO>(null, 0, (await CheckId(id)).Code, (await CheckId(id)).Message);
			var result = await _roleManager.FindByIdAsync(id);
			return new Response<RoleDTO>(_mapper.Map<RoleDTO>(result), 1, HttpStatusCode.OK, null);
		}
		public async Task<Response<RoleDTO>> GetByName(string name)
		{
			if ((await CheckName(name)).Content == false)
				return new Response<RoleDTO>(null, 0, (await CheckName(name)).Code, (await CheckName(name)).Message);
			var result = await _roleManager.FindByNameAsync(name);
			return new Response<RoleDTO>(_mapper.Map<RoleDTO>(result), 1, HttpStatusCode.OK, null);
		}
		public async Task<Response<bool>> Create(string roleName)
		{
			if (await _context.Roles.AnyAsync(x => x.Name == roleName))
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, $"Esiste già un ruolo: {roleName}");
			ApplicationRole role = new ApplicationRole();
			role.Name = roleName;
			role.IsDeleted = false;
			var addRoleResult = await _roleManager.CreateAsync(role);
			return Response(addRoleResult, "Creato con succusso", HttpStatusCode.Created);
		}
		public async Task<Response<bool>> Update(RoleDTO role)
		{
			var _role = await _roleManager.FindByIdAsync(role.Id);
			if (_role == null)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, $"Nessun ruolo con Id = {role.Id}");
			var updateRoleResult = await _roleManager.UpdateAsync(_mapper.Map<ApplicationRole>(role));
			return Response(updateRoleResult, "Modificato con succusso");
		}
		public async Task<Response<bool>> Delete(string roleId)
		{
			var _role = await _roleManager.FindByIdAsync(roleId);
			if (_role == null)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, $"Nessun ruolo con Id = {roleId}");
			_role.IsDeleted = true;
			var result = await _roleManager.DeleteAsync(_role);
			return Response(result, "Ruolo disattivato");
		}
		public async Task<Response<bool>> Deactive(string roleId)
		{
			var _role = await _roleManager.FindByIdAsync(roleId);
			if (_role == null)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, $"Nessun ruolo con Id = {roleId}");
			_role.IsDeleted = true;
			var result = await _roleManager.UpdateAsync(_role);
			return Response(result, "Ruolo disattivato");
		}
		public async Task<Response<bool>> Reactive(string roleId)
		{
			var _role = await _roleManager.FindByIdAsync(roleId);
			if (_role == null)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, $"Nessun ruolo con Id = {roleId}");
			_role.IsDeleted = false;
			var result = await _roleManager.UpdateAsync(_role);
			return Response(result, "Ruolo disattivato");
		}
		public async Task<Response<IList<string>>> GetRolesUser(string userId)
		{
			var _user = await _userManager.FindByIdAsync(userId);
			if (_user == null)
				return new Response<IList<string>>(null, 0, HttpStatusCode.UnprocessableEntity, $"Nessun utente con Id = {userId}");
			var result = await _userManager.GetRolesAsync(_user);
			return new Response<IList<string>>(result, result.Count, HttpStatusCode.OK, null);
		}
		public async Task<Response<bool>> AddRoleUser(UserRoleDTO user)
		{
			if ((await CheckName(user.RoleName)).Content == false)
				return await CheckName(user.RoleName);
			var _user = await _userManager.FindByIdAsync(user.userId);
			if (_user == null)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, $"Nessun utente con Id = {user.userId}");
			if (await _userManager.IsInRoleAsync(_user, user.RoleName))
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, $"Questo utente è già un {user.RoleName}");
			var result = await _userManager.AddToRoleAsync(_user, user.RoleName);
			return Response(result, $"{_user.UserName} aggiunto in {user.RoleName}");
		}
		public async Task<Response<bool>> DelRoleUser(UserRoleDTO user)
		{
			if ((await CheckName(user.RoleName)).Content == false)
				return await CheckName(user.RoleName);
			var _user = await _userManager.FindByIdAsync(user.userId);
			if (_user == null)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, $"Nessun utente con Id = {user.userId}");
			if (await _userManager.IsInRoleAsync(_user, user.RoleName) == false)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, $"Questo utente non è un {user.RoleName}");
			var result = await _userManager.RemoveFromRoleAsync(_user, user.RoleName);
			return Response(result, $"{_user.UserName} aggiunto in {user.RoleName}");
		}
		#endregion

		#region UTILITIES
		private async Task<Response<bool>> CheckId(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Id non è valido!");
			if (await _context.Roles.AnyAsync(x => x.Id == id) == false)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, $"Nessuno con Id = {id}");
			return new Response<bool>(true, 0, HttpStatusCode.OK, null);
		}
		private async Task<Response<bool>> CheckName(string nome)
		{
			if (string.IsNullOrWhiteSpace(nome))
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Nome non è valido!");
			if (await _context.Roles.AnyAsync(x => x.Name == nome) == false)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, $"Nessuno con Nome = {nome}");
			return new Response<bool>(true, 0, HttpStatusCode.OK, null);
		}
		public async Task<bool> AnyoneWithRole(string id)
		{
			return await _context.UserRoles.AnyAsync(x => x.RoleId == id);
		}
		private Response<List<RoleDTO>> Response(List<ApplicationRole> data, int count)
		{
			if (count == 0)
				return new Response<List<RoleDTO>>(null, 0, HttpStatusCode.OK, "Nessun ruolo trovato");
			return new Response<List<RoleDTO>>(data.ConvertAll(_mapper.Map<RoleDTO>), count, HttpStatusCode.OK, null);
		}
		private Response<bool> Response(IdentityResult result, string message, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			if (result.Succeeded)
				return new Response<bool>(true, 1, statusCode, message);
			return new Response<bool>(false, 0, HttpStatusCode.InternalServerError, ErrorsResponse(result.Errors));
		}
		public string ErrorsResponse(IEnumerable<IdentityError> errors)
		{
			string response = "";
			if (errors.Count() == 0)
				return response;
			foreach (var item in errors)
			{
				response += item.Description + "  ";
			}
			return response;
		}
		#endregion
	}
}
