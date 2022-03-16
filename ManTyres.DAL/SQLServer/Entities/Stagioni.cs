using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ManTyres.DAL.SQLServer.Entities
{
	public partial class Stagioni
	{
		public Stagioni()
		{
			Pneumaticis = new HashSet<Pneumatici>();
		}
		[Key]
		public int StagioneId { get; set; }
		public string Nome { get; set; }

		public virtual ICollection<Pneumatici> Pneumaticis { get; set; }
	}
}
