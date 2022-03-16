using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ManTyres.DAL.SQLServer.Entities
{
	public partial class Inventario
	{
		[Key]
		public int PneumaticiId { get; set; }
		[Key]
		public DateTime InizioDeposito { get; set; }
		public DateTime? FineDeposito { get; set; }
		public int? DepositoId { get; set; }
		[Column(TypeName = "int")]
		public decimal Battistrada { get; set; }
		public string StatoGomme { get; set; }
		public string UserId { get; set; }
		public bool IsDeleted { get; set; }

		[ForeignKey("DepositoId")]
		public virtual Depositi Deposito { get; set; }
		[ForeignKey("PneumaticiId")]
		public virtual Pneumatici Pneumatici { get; set; }
		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; }
	}
}
