using System;
using System.Collections.Generic;
using System.Text;

namespace ManTyres.COMMON.DTO
{
	#nullable disable

	public class PneumaticiDTO
	{
		public int PneumaticiId { get; set; }
		public string Marca { get; set; }
		public string Modello { get; set; }
		public string Misura { get; set; }
		public string Dot { get; set; }
		public int StagioneId { get; set; }
		public int VeicoloId { get; set; }
		public DateTime DataUbicazione { get; set; }
		public decimal Quantita { get; set; }
		public bool IsDeleted { get; set; }

		public StagioniDTO Stagione { get; set; }
		public VeicoliDTO Veicolo { get; set; }
	}
}
