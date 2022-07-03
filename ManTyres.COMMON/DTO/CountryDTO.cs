

namespace ManTyres.COMMON.DTO
{
	public class CountryDTO
	{
      public string? Id { get; set; }
		public string? Name { get; set; }
		public string? Country_Code { get; set; }
		public string? ISO_Codes { get; set; }
		public string? ISO2 { get; set; }
		public string? ISO3 { get; set; }
		public string? CurrencyCode { get; set; }
		public string? Population { get; set; }
		public string? Capital { get; set; }
		public string? ContinentName { get; set; }
	}

	public class NewCountryDTO
	{
      public string? CountryCode { get; set; }
		public string? CountryName { get; set; }
		public string? CurrencyCode { get; set; }
		public string? Population { get; set; }
		public string? Capital { get; set; }
		public string? ContinentName { get; set; }
	}
}
