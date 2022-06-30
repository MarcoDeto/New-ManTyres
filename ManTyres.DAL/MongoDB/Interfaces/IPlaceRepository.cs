﻿using ManTyres.COMMON.DTO;
using ManTyres.DAL.Infrastructure.MongoDB.Interfaces;
using ManTyres.DAL.MongoDB.Models;

namespace ManTyres.DAL.MongoDB.Interfaces
{
   public interface IPlaceRepository : IBaseRepository<Place>
   {
      Task<List<Place>> GetWhereIsNull();
      Task<List<Place>> GetByPlacesId(string[] places_id);
      Task<bool> ExistByName(string name);
      Task<bool> InsertMany(List<Place> entity);
   }
}
