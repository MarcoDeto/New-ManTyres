using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ManTyres.DAL.SQLServer.Entities
{
	public class ApplicationRole : IdentityRole
	{
		[Required, Column(TypeName = "bit")]
		public bool IsDeleted { get; set; }
	}
}
