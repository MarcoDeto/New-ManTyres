using ManTyres.COMMON.Utils;
using System.Threading.Tasks;

namespace ManTyres.COMMON.Interfaces
{
	public interface IGoogleGeoCodingService
	{
		Task<DataBridge<GoogleGeoCodingResponse>> GeoCodeAsync(string address);
	}
}
