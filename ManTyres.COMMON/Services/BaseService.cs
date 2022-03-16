using Newtonsoft.Json;

namespace ManTyres.COMMON.Services
{
	public class BaseService
	{
		public static T Clone<T>(T source)
		{
			var serialized = JsonConvert.SerializeObject(
				source,
				new JsonSerializerSettings
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
					//NullValueHandling = NullValueHandling.Ignore,
					//MissingMemberHandling = MissingMemberHandling.Ignore,
				});
			return JsonConvert.DeserializeObject<T>(serialized);
		}
	}
}
