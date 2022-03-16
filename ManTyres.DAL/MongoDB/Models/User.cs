using ManTyres.COMMON.Utils;

namespace ManTyres.DAL.MongoDB.Models
{
   public class User : MongoDocument
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
}
