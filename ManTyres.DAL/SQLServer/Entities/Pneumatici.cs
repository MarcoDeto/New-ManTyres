using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ManTyres.DAL.SQLServer.Entities
{
	public partial class Pneumatici
	{
		public Pneumatici()
		{
			Inventarios = new HashSet<Inventario>();
		}
		[Key]
		public int PneumaticiId { get; set; }
		public string Marca { get; set; }
		public string Modello { get; set; }
		public string Misura { get; set; }
		public string Dot { get; set; }
		public int StagioneId { get; set; }
		public int VeicoloId { get; set; }
		public DateTime DataUbicazione { get; set; }
		[Column(TypeName = "int")]
		public decimal Quantità { get; set; }
		public bool IsDeleted { get; set; }

		[ForeignKey("StagioneId")]
		public virtual Stagioni Stagione { get; set; }
		[ForeignKey("VeicoloId")]
		public virtual Veicoli Veicolo { get; set; }
		public virtual ICollection<Inventario> Inventarios { get; set; }
	}
}
