using ManTyres.COMMON.Utils;

namespace ManTyres.COMMON.DTO
{
	public class ClaimLogDTO : BaseDTO
	{
		public int IdClaim { get; set; }
		public int? UserId { get; set; }
		public string? EmailVsg { get; set; }
		public string? FullName { get; set; }
		public UserRole UserRole { get; set; }
		public ClaimStatus Status { get; set; }
		public string? Note { get; set; }

		public ClaimDTO? Claim { get; set; }
		public UserDTO? User { get; set; }
	}
}
