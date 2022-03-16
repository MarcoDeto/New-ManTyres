using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManTyres.DAL.SQLServer.Entities
{
	public class ApplicationUser : IdentityUser
	{
		[Required, Column(TypeName = "nvarchar(50)")]
		public string FirstName { get; set; }

		[Required, Column(TypeName = "nvarchar(50)")]
		public string LastName { get; set; }

		public byte[] ImgProfile { get; set; }

		[Required, Column(TypeName = "bit")]
		public bool IsDeleted { get; set; }
	}
}
