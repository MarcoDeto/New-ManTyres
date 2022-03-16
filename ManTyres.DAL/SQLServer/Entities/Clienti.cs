using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ManTyres.DAL.SQLServer.Entities
{
	public partial class Clienti
	{
		public Clienti()
		{
			Veicolis = new HashSet<Veicoli>();
		}

		[Key]
		public int ClienteId { get; set; }
		public string Cognome { get; set; }
		public string Nome { get; set; }
		public string CodiceFiscale { get; set; }
		public string PartitaIva { get; set; }
		public string Nazione { get; set; }
		public string Provincia { get; set; }
		public string Cap { get; set; }
		public string Comune { get; set; }
		public string Indirizzo { get; set; }
		public string Civico { get; set; }
		public string Email { get; set; }
		public string Telefono { get; set; }
		public bool IsAzienda { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime DataCreazione { get; set; }

		public virtual ICollection<Veicoli> Veicolis { get; set; }
	}
}
