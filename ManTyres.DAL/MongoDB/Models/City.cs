using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManTyres.DAL.MongoDB.Models
{
	public class City
	{
      [BsonId]
      [BsonRepresentation(BsonType.ObjectId)]
      public ObjectId Id { get; set; }
		public string? Name { get; set; }
		public string? ASCII { get; set; }
		public float LAT { get; set; }
		public float LNG { get; set; }
		public string? Country { get; set; }
      [MaxLength(2)]
		public string? ISO2 { get; set; }
      [MaxLength(3)]
		public string? ISO3 { get; set; }
		public string? Admin_name { get; set; }
		public int Population { get; set; }
	}
}
