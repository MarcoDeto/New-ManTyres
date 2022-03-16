using System.ComponentModel.DataAnnotations;
using ManTyres.COMMON.Utils;

namespace ManTyres.COMMON.DTO
{
	public class UserDTO : BaseDTO
	{
      public bool TwoFactorEnabled { get; set; }
      public bool PhoneNumberConfirmed { get; set; }
      public string? PhoneNumber { get; set; }
      public string? ConcurrencyStamp { get; set; }
      public string? SecurityStamp { get; set; }
      public string? PasswordHash { get; set; }
      public bool EmailConfirmed { get; set; }
      public string? Email { get; set; }
      public string? UserName { get; set; }
      public string? FirstName { get; set; }
      public string? LastName { get; set; }
      public string? PhotoUrl { get; set; }
      public string? CompanyName { get; set; }
      public string? Street { get; set; }
      public string? Zipcode { get; set; }
      public string? Country { get; set; }
      public string? City { get; set; }
      public string? Website { get; set; }
      public string? CultureInfo { get; set; }
      public string? Provider { get; set; }
      public UserRole Role { get; set; }
	}

	public class UserNameDTO
	{
		public string Id { get; set; }
		public byte[] ImgProfile { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
	}

	public class UserPasswordDTO
	{
		public string Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string NewPassword { get; set; }
	}

	public class UserRoleDTO
	{
		public string userId { get; set; }
		public string RoleName { get; set; }
	}
}
