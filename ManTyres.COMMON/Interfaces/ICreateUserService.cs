using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.COMMON.Interfaces
{
	public interface ICreateUserService
	{
		Task<User> CreateUser(User user);
		string _generateRandomPassword();
	}
}
