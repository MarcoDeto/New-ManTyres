using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManTyres.COMMON.Utils
{
   public class EmailOptions {

      public const string EmailVSG = "EmailVSG";

      public string Recipient { get; set; }
      public string Sender { get; set; }
   }

   public class GoogleOptions
	{
      public const string Section = "GoogleMaps";
      public string ApiKey { get; set; }
      public string GeoCodingBaseUrl { get; set; }
   }
}
