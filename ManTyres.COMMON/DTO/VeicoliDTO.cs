using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ManTyres.COMMON.DTO
{

#nullable disable

	public class VeicoliDTO
	{
		public int VeicoloId { get; set; }
		[Required(ErrorMessage = "Targa è obbligatoria")]
		public string Targa { get; set; }
		public string Marca { get; set; }
		public string Modello { get; set; }
		public int? ClienteId { get; set; }
		public bool IsDeleted { get; set; }
		public ClientiDTO Cliente { get; set; }
		public DateTime DataCreazione { get; set; }
	}

	public class TabVeicoliDTO
	{
		public int VeicoloId { get; set; }
		public string Targa { get; set; }
		public string Marca { get; set; }
		public string Modello { get; set; }
		public int ClienteId { get; set; }
		public bool IsDeleted { get; set; }
		public string CodiceFiscale { get; set; }
	}
}
