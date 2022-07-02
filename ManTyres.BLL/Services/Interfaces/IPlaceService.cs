using ManTyres.BLL.Services;
using ManTyres.COMMON.DTO;
using ManTyres.DAL.MongoDB.Models;
using ManTyres.DAL.MongoDB.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Interfaces
{
	public interface IPlaceService : IBaseService<PlaceDTO, PlaceRepository, Place>
	{
		Task<Response<List<PlaceDTO>>> GetWhereIsNull();
		Task<Response<List<PlaceDTO>>> GetNear(double LAT, double LNG);
		Task<Response<List<PlaceDTO>>> GetByPlacesId(string[] places_id);
		Task<Response<bool>> AddPlaces(List<PlaceDTO> places);
		Task<Response<bool>> AddPlace(PlaceDTO place);
	}
}