namespace ManTyres.DAL.Infrastructure.MongoDB.Interfaces
{
	#nullable disable
	public class MongoSettings : IMongoSettings
	{
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }
	}
	public interface IMongoSettings
	{
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }
	}
}
