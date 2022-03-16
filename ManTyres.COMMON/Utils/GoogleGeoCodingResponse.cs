using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ManTyres.COMMON.Utils
{
    public class GoogleGeoCodingResponse
    {
        public IEnumerable<GoogleGeoCodeResult>? Results { get; set; }
        public string? Status { get; set; }
    }

    public class GoogleGeoCodeResult
    {
        [JsonPropertyName("address_components")]
        public IEnumerable<AddressComponent>? AddressComponents { get; set; }
        public Geometry? Geometry { get; set; }

    }

    public class Geometry
    {
        public Location? Location { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class AddressComponent
    {
        [JsonPropertyName("long_name")]
        public string? LongName { get; set; }
        [JsonPropertyName("short_name")]
        public string? ShortName { get; set; }
        public IEnumerable<string>? Types { get; set; }
    }
}
