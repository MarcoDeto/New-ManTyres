using System.ComponentModel.DataAnnotations;

namespace ManTyres.DAL.MongoDB.Models
{
	public class Car : MongoDocument
	{
		public string? Company { get; set; }
		public string? Model { get; set; }
		public string? Generation { get; set; }

      public int? Year_from { get; set; }
      public int? Year_to { get; set; }
      public string? Series { get; set; }
      public string? Trim { get; set; }
      public string? Body_type { get; set; }
		public int? number_of_seats { get; set; }
		public string? Cylinder_layout { get; set; }
      public int? Number_of_cylinders { get; set; }
      public string? Engine_type { get; set; }
      public int? Valves_per_cylinder { get; set; }
      public string? Boost_type { get; set; }
      public string? Transmission { get; set; }
      public string[]? Wheel_sizes { get; set; }
	}
}

