using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ManTyres.COMMON.DTO
{

#nullable disable

	public class SediDTO
	{
		[Required(ErrorMessage = "SedeId è obbligatorio")]
		public int SedeId { get; set; }
		public string Nazione { get; set; }
		public string Provincia { get; set; }
		public string Cap { get; set; }
		[Required(ErrorMessage = "Comune è obbligatorio")]
		public string Comune { get; set; }
		public string Indirizzo { get; set; }
		public string Civico { get; set; }
		public string Telefono { get; set; }
		public bool IsDeleted { get; set; }
	}
}
