using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ManTyres.DAL.SQLServer.Entities
{
	public partial class Sedi
	{
		public Sedi()
		{
			Depositis = new HashSet<Depositi>();
		}
		[Key]
		public int SedeId { get; set; }
		public string Nazione { get; set; }
		public string Provincia { get; set; }
		public string Cap { get; set; }
		public string Comune { get; set; }
		public string Indirizzo { get; set; }
		public string Civico { get; set; }
		public string Telefono { get; set; }
		public bool IsDeleted { get; set; }

		public virtual ICollection<Depositi> Depositis { get; set; }
	}
}
