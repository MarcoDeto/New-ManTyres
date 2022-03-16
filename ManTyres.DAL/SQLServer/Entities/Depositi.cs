using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ManTyres.DAL.SQLServer.Entities
{
	public partial class Depositi
	{
		public Depositi()
		{
			Inventarios = new HashSet<Inventario>();
		}
		[Key]
		public int DepositoId { get; set; }
		public int SedeId { get; set; }
		public string Ubicazione { get; set; }
		public bool IsDeleted { get; set; }

		[ForeignKey("SedeId")]
		public virtual Sedi Sede { get; set; }
		public virtual ICollection<Inventario> Inventarios { get; set; }
	}
}
