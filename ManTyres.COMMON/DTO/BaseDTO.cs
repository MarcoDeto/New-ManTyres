using System;
using MongoDB.Bson;

namespace ManTyres.COMMON.DTO
{
	#nullable disable
	public class BaseDTO
	{
		public string Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public bool IsDeleted { get; set; }
	}
}
