using ManTyres.BLL.Services;
using ManTyres.COMMON.DTO;
using ManTyres.DAL.MongoDB.Models;
using ManTyres.DAL.MongoDB.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface IUserService : IBaseService<UserDTO, UserRepository, User>
	{
		Task<Response<bool>> CreateAccount(UserDTO request);
		Task<Response<UserDTO?>> FindByEmail(string email);
		Task<Response<UserDTO?>> CheckLogin(LoginDTO request);
	}
}
