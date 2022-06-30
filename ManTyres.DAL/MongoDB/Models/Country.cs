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
		public string? Country_Code { get; set; }
		public string? ISO_Codes { get; set; }
		public string? ISO2 { get; set; }
		public string? ISO3 { get; set; }
	}
}