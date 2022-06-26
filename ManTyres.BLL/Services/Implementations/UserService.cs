using AutoMapper;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using ManTyres.COMMON.Utils;
using ManTyres.DAL.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ManTyres.BLL.Services.Implementations
{
   public class UserService : BaseService<UserDTO, IUserRepository, User>, IUserService
   {
      private readonly IUserRepository _userRepository;
      private readonly IMapper _mapper;
      private readonly ILogger<IUserService> _logger;

      public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger) : base(userRepository, mapper, logger)
      {
         _mapper = mapper;
         _userRepository = userRepository;
         _logger = logger;
         _logger.LogDebug("ctor");
      }

      #region METHODS
      public async Task<Response<bool>> CreateAccount(UserDTO request)
      {
			if (request == null)
            return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "ERROR_NULL");
         User toAdd = _mapper.Map<User>(request);
			if (toAdd != null && string.IsNullOrWhiteSpace(toAdd.Email))
            return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "ERROR_NULL");
         else if (await _userRepository.IsAlreadyExists(toAdd!.Email!))
            return new Response<bool>(false, 0, HttpStatusCode.Conflict, "ERROR_EmailAlreadyExist");
         var _user = new UserDTO()
         {
            PhotoUrl = request.PhotoUrl,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Provider = request.Provider,
            UserName = request.UserName,
            IsDeleted = false,
            EmailConfirmed = false,
            //Id = Guid.NewGuid().ToString(),
            PasswordHash = request.PasswordHash,
            Role = UserRole.Administrator
         };
         return await base.PostReturnBool(request);
      }

      public async Task<Response<UserDTO?>> FindByEmail(string email)
      {
         if (await _userRepository.IsAlreadyExists(email) == false)
            return new Response<UserDTO?>(null, 0, HttpStatusCode.NotFound, "ERROR_USERNOTFOUND");
			
			var user = await _userRepository.GetByEmail(email);
			return new Response<UserDTO?>(_mapper.Map<UserDTO>(user), 1, HttpStatusCode.OK, "SUCCESS_GET");
      }

      public async Task<Response<UserDTO?>> CheckLogin(LoginDTO request)
      {
         if (await _userRepository.IsAlreadyExists(request.Email) == false)
            return new Response<UserDTO?>(null, 0, HttpStatusCode.NotFound, "ERROR_USERNOTFOUND");
			
         if (await _userRepository.CheckPassword(request) == false)
            return new Response<UserDTO?>(null, 0, HttpStatusCode.Unauthorized, "ERROR_PASSWORD");

         var user = await _userRepository.GetByEmail(request.Email);
			return new Response<UserDTO?>(_mapper.Map<UserDTO>(user), 1, HttpStatusCode.OK, "SUCCESS");
      }

      #endregion



      #region JSON-METHODS
      public async Task<Response<List<UserDTO>>> GetAllUsersJSON()
      {
         string json = File.ReadAllText("users.json");
         List<UserDTO> result = JsonConvert.DeserializeObject<List<UserDTO>>(json);
         result = result.Where(x => x.Role != UserRole.Administrator).ToList();
         if (result.Count == 0)
            return new Response<List<UserDTO>>(null, 0, HttpStatusCode.OK, "Non ci sono utenti");
         return new Response<List<UserDTO>>(result, result.Count, HttpStatusCode.OK, null);
      }
      public async Task<Response<UserDTO>> GetUserByIdJSON(string id)
      {
         string json = File.ReadAllText("users.json");
         List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
         /*var user = users.Find(x => x.Id == id);
			if (user == null)
				return new Response<UserDTO>(null, 0, HttpStatusCode.NotFound, $"Nessun utente con Id = {id}");
			return new Response<UserDTO>(user, 1, HttpStatusCode.OK, null);*/
         return null;
      }
      public async Task<Response<UserDTO>> GetUserbyUsernameJSON(string username)
      {
         string json = File.ReadAllText("users.json");
         List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
         var user = users.Find(x => x.UserName == username);
         if (user == null)
            return new Response<UserDTO>(null, 0, HttpStatusCode.NotFound, $"Nessun utente con Username = {username}");
         var response = new UserDTO
         {
            PhotoUrl = user.PhotoUrl,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Provider = user.Provider,
            UserName = user.Email,
            IsDeleted = false,
            EmailConfirmed = false,
            PasswordHash = user.PasswordHash,
            Role = UserRole.Administrator
         };
         return new Response<UserDTO>(response, 1, HttpStatusCode.OK, null);
      }
      public async Task<Response<bool>> CreateUserJSON(UserDTO user)
      {
         if (await IsAlreadyExistsJSON(user) != null)
            return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, await IsAlreadyExistsJSON(user));
         var _user = new UserDTO()
         {
            PhotoUrl = user.PhotoUrl,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Provider = user.Provider,
            UserName = user.Email,
            IsDeleted = false,
            EmailConfirmed = false,
            PasswordHash = user.PasswordHash,
            Role = UserRole.Administrator
         };
         string json = File.ReadAllText("users.json");
         List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
         users.Add(_user);
         File.WriteAllText("users.json", JsonConvert.SerializeObject(users));
         return new Response<bool>(true, 1, HttpStatusCode.Created, "Creato con succusso");
      }
      public async Task<Response<bool>> UpdateUserJSON(UserDTO user)
      {
         string json = File.ReadAllText("users.json");
         List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
         var _user = users.Find(x => x.Id == user.Id);
         if (_user == null)
            return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, $"Nessun utente con Id = {user.Id}");
         if (await IsAlreadyExistsJSON(user) != null)
            return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, await IsAlreadyExistsJSON(user));
         _user.PhotoUrl = user.PhotoUrl;
         _user.FirstName = user.FirstName;
         _user.LastName = user.LastName;
         _user.UserName = user.UserName;
         _user.Email = user.Email;
         _user.PhoneNumber = user.PhoneNumber;
         _user.IsDeleted = user.IsDeleted;
         UserDTO toDelete = users.Find(x => x.Id == user.Id);
         users.Remove(toDelete);
         users.Add(user);
         File.WriteAllText("users.json", JsonConvert.SerializeObject(users));
         return new Response<bool>(true, 1, HttpStatusCode.OK, "Modificato con succusso");
      }
      public async Task<Response<bool>> CheckCurrentPasswordJSON(UserPasswordDTO user)
      {
         string json = File.ReadAllText("users.json");
         List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
         var _user = users.Find(x => x.UserName == user.Username);
         if (_user == null)
            return new Response<bool>(false, 0, HttpStatusCode.NotFound, $"Nessun utente con Username = {user.Username}");
         if (_user.PasswordHash == user.Password)
            return new Response<bool>(true, 1, HttpStatusCode.OK, "Password corretta");
         return new Response<bool>(false, 1, HttpStatusCode.BadRequest, "Password sbagliata");
      }
      public async Task<Response<bool>> ChangePasswordJSON(UserPasswordDTO user)
      {
         string json = File.ReadAllText("users.json");
         List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
         var _user = users.Find(x => x.UserName == user.Username);
         if (_user == null)
            return new Response<bool>(false, 0, HttpStatusCode.NotFound, $"Nessun utente con Username = {user.Username}");
         //UserDTO toDelete = users.Find(x => x.Id == user.Id);
         //users.Remove(toDelete);
         _user.PasswordHash = user.Password;
         users.Add(_user);
         File.WriteAllText("users.json", JsonConvert.SerializeObject(users));
         return new Response<bool>(true, 1, HttpStatusCode.OK, "Password è stata modificata");
      }
      public async Task<Response<bool>> ResetPasswordJSON(UserPasswordDTO user)
      {
         string json = File.ReadAllText("users.json");
         List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
         /*var _user = users.Find(x => x.Id == user.Id);
			if (_user == null)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, $"Nessun utente con Id = {user.Id}");
			users.Remove(_user);
			_user.PasswordHash = user.Password;
			users.Add(_user);
			File.WriteAllText("users.json", JsonConvert.SerializeObject(users));*/
         return new Response<bool>(true, 1, HttpStatusCode.OK, "Password è stata modificata");
      }
      public async Task<Response<bool>> DeactiveJSON(string userId)
      {
         string json = File.ReadAllText("users.json");
         List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
         /*var _user = users.Find(x => x.Id == userId);
			if (_user == null)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, $"Nessun utente con Id = {userId}");
			users.Remove(_user);
			_user.IsDeleted = true;
			users.Add(_user);
			File.WriteAllText("users.json", JsonConvert.SerializeObject(users));*/
         return new Response<bool>(true, 1, HttpStatusCode.OK, "Utente disattivato");
      }
      public async Task<Response<bool>> ReactiveJSON(string userId)
      {
         string json = File.ReadAllText("users.json");
         List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
         /*var _user = users.Find(x => x.Id == userId);
			if (_user == null)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, $"Nessun utente con Id = {userId}");
			users.Remove(_user);
			_user.IsDeleted = false;
			users.Add(_user);
			File.WriteAllText("users.json", JsonConvert.SerializeObject(users));*/
         return new Response<bool>(true, 1, HttpStatusCode.OK, "Utente riattivato");
      }
      #endregion

      #region JSON-UTILITIES
      private async Task<string> IsAlreadyExistsJSON(UserDTO user)
      {
         string json = File.ReadAllText("users.json");
         List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
         if (users.Any(x => x.Id != user.Id && x.UserName == user.UserName))
            return "Username già esistente!";
         else if (users.Any(x => x.Id != user.Id && x.Email == user.Email))
            return $"Email già esistente!";
         return null;
      }
      public async Task<bool> IsAlreadyDisactivedJSON(string userId)
      {
         string json = File.ReadAllText("users.json");
         List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
         var result = users.Find(x => x.Id == userId);
         if (result != null)
            return result.IsDeleted;
         return false;
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
