using System;
using System.ComponentModel.DataAnnotations;

namespace ManTyres.COMMON.DTO
{
	#nullable disable

	public class ClientiDTO
	{
		[Required(ErrorMessage = "ClienteId è obbligatorio")]
		public int ClienteId { get; set; }
		public string Cognome { get; set; }
		[Required(ErrorMessage = "Nome è obbligatorio")]
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
	}

	public class SelectClientiDTO
	{
		public int ClienteId { get; set; }
		public string Cognome_Nome { get; set; }
	}
}
