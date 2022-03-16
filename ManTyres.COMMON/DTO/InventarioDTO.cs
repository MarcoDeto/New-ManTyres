using System;
using System.Collections.Generic;
using System.Text;

namespace ManTyres.COMMON.DTO
{
	#nullable disable

	public class InventarioDTO
	{
		public int PneumaticiId { get; set; }
		public DateTime InizioDeposito { get; set; }
		public DateTime? FineDeposito { get; set; }
		public int? DepositoId { get; set; }
		public decimal Battistrada { get; set; }
		public string StatoGomme { get; set; }
		public string UserId { get; set; }
		public bool IsDeleted { get; set; }

		public PneumaticiDTO Pneumatici { get; set; }
		public DepositiDTO Deposito { get; set; }
		public UserDTO User { get; set; }
	}
}
