using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ManTyres.COMMON.DTO
{
	#nullable disable

	public class DepositiDTO
	{
		public int DepositoId { get; set; }
		[Required(ErrorMessage = "SedeId è obbligatorio")]
		public int SedeId { get; set; }
		[Required(ErrorMessage = "Ubicazione è obbligatoria")]
		public string Ubicazione { get; set; }
		public bool IsDeleted { get; set; }

		public SediDTO Sede { get; set; }
	}
}
