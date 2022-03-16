using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ManTyres.DAL.MongoDB.Models
{
   public class MongoDocument
   {
      [BsonId]
      [BsonRepresentation(BsonType.ObjectId)]
      public ObjectId Id { get; set; }
      public DateTime CreatedAt { get; set; }
      public DateTime UpdatedAt { get; set; }
      public bool IsDeleted { get; set; }
   }
}
