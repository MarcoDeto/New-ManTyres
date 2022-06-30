using System.ComponentModel.DataAnnotations;

namespace ManTyres.DAL.MongoDB.Models
{
	public class Place : MongoDocument
	{		
      public float LAT { get; set; }
      public float LNG { get; set; }
		public string? Address { get; set; }
		public string? Locality { get; set; }
		public string? Administrative_area_level_1 { get; set; }
      public string? Administrative_area_level_2 { get; set; }
      public string? Administrative_area_level_3 { get; set; }
      public string? Country { get; set; }

      [MaxLength(2)]
		public string? ISO2 { get; set; }
		public string? Postal_code { get; set; }
		public string? Business_status { get; set; }
      public string? Name { get; set; }
      public string? Website { get; set; }
      public bool Open_Now { get; set; }
      public Period[]? Periods { get; set; }
      public string[]? Weekday_Text { get; set; }
		public string? Phone_Number { get; set; }
      public string? International_Phone_Number { get; set; }
      public string[]? Google_Photos { get; set; }
      public string? Google_Place_Id { get; set; }
      public float Google_Rating { get; set; }
      public string? Google_Url { get; set; }
      public string? Email { get; set; }
      public bool Verified { get; set; }
	}

   public class Period 
   {
      public Hours? Open  { get; set; }
      public Hours? Close  { get; set; }
   }
   public class Hours
   {
      public int? Day  { get; set; }
      public string? time  { get; set; }
   }
}

