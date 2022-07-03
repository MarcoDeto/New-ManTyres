using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManTyres.DAL.MongoDB.Models
{
	public class Country
	{
      [BsonId]
      [BsonRepresentation(BsonType.ObjectId)]
      public ObjectId Id { get; set; }
		public string? Name { get; set; }
		public string? CountryCode { get; set; }
		public string? ISOCodes { get; set; }
		public string? ISO2 { get; set; }
		public string? ISO3 { get; set; }
		public string? CurrencyCode { get; set; }
		public string? Population { get; set; }
		public string? Capital { get; set; }
		public string? ContinentName { get; set; }
	}
}
