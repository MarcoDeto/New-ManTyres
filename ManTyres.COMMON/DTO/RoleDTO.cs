using System;
using System.Collections.Generic;
using System.Text;

namespace ManTyres.COMMON.DTO
{
	#nullable disable

	public class RoleDTO
	{
		public string Id { get; set; }
		public bool IsDeleted { get; set; }
		public string Name { get; set; }
		public string NormalizedName { get; set; }
		public string ConcurrencyStamp { get; set; }
	}
}
