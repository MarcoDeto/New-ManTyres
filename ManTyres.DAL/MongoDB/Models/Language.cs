using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManTyres.DAL.MongoDB.Models
{
	public class Language
	{
      [BsonId]
      [BsonRepresentation(BsonType.ObjectId)]
      public ObjectId Id { get; set; }
		public string? Name { get; set; }
		public string? Code { get; set; }
		public string? Country { get; set; }
		public string? Emoji { get; set; }
	}
}
