using ManTyres.COMMON.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ManTyres.COMMON.DTO
{
	public class ClaimDTO : BaseDTO
	{
		public DateTime? InstallationDate { get; set; }
		public DateTime? FailureDate { get; set; }
		public string? CustomerClaimCode { get; set; }
		public string? Defect { get; set; }
		public string? DefectDescription { get; set; }
		public string? BlobFolderPath { get; set; }
		public string? ContactPerson { get; set; }
		public string? ContactPhone { get; set; }
		public string? ContactEmail { get; set; }
		public DateTime? DateOpened { get; set; }
		public DateTime? DateClosed { get; set; }
		public string? ShippingAddress { get; set; }
		public ClaimStatus Status { get; set; }

		public virtual ICollection<ClaimLogDTO>? ClaimLogs { get; set; }
	}
	public class PostClaimDTO
	{
		public string? SerialNumber { get; set; }
		public string? Claim { get; set; }
		public string? Log { get; set; }
	}

	public class PutClaimDTO
	{
		public List<IFormFile>? Files { get; set; }
		public string? Claim { get; set; }
		public string? Log { get; set; }
		public string? InvolvedParts { get; set; }
		public string? Tag { get; set; }
	}

	public class GetClaimDTO
	{
		public ClaimDTO? Claim { get; set; }
		public List<FileDTO>? Files { get; set; }
	}

	public class FileDTO
	{
		public string? Name { get; set; }
		public string? Path { get; set; }
		public long? Size { get; set; }
		public string? Type { get; set; }
		public DateTimeOffset? LastModified { get; set; }
		public bool Uploaded { get; set; }

		public FileDTO(string name, string path, long? size, string type, DateTimeOffset? lastModified)
		{
			Name = name;
			Path = path;
			Size = size;
			Type = type;
			LastModified = lastModified;
			Uploaded = true;
		}
	}
}
