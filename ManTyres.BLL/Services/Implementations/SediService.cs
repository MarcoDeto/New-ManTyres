using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Implementations
{
	public class SediService : ISediService
	{
		public SediService() { }

		#region METHODS
		public async Task<Response<List<SediDTO>>> GetAllSedi()
		{
			string json = File.ReadAllText("sedi.json");
			List<SediDTO> sedi = JsonConvert.DeserializeObject<List<SediDTO>>(json);
			var data = sedi.Where(x => x.IsDeleted == false).OrderBy(x => x.Comune).ToList();
			if (data.Count == 0)
				return new Response<List<SediDTO>>(null, 0, HttpStatusCode.OK, "Nessuna sede trovato");
			return new Response<List<SediDTO>>(data, data.Count, HttpStatusCode.OK, null);
		}
		public async Task<Response<SediDTO>> GetById(int id)
		{
			string json = File.ReadAllText("sedi.json");
			List<SediDTO> sedi = JsonConvert.DeserializeObject<List<SediDTO>>(json);
			if (sedi.Any(x => x.SedeId == id && x.IsDeleted == false) == false)
				return new Response<SediDTO>(null, 1, HttpStatusCode.NotFound, $"Nessuna sede con Id = {id}");
			var sede = sedi.FirstOrDefault(x => x.SedeId == id && x.IsDeleted == false);
			return new Response<SediDTO>(sede, 1, HttpStatusCode.OK, null);
		}
		public async Task<Response<bool>> AddSede(SediDTO sede)
		{
			string json = File.ReadAllText("sedi.json");
			List<SediDTO> sedi = JsonConvert.DeserializeObject<List<SediDTO>>(json);
			if (sedi.Any(x => x.Comune == sede.Comune && x.Indirizzo == sede.Indirizzo
													  && x.Civico == sede.Civico && x.IsDeleted == false))
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Sede già esistente");
			var _sede = sedi.SingleOrDefault(x => x.Comune == sede.Comune && x.Indirizzo == sede.Indirizzo
																				 && x.Civico == sede.Civico && x.IsDeleted == true);
			if (_sede != null)
			{
				_sede = new SediDTO()
				{
					Cap = sede.Cap,
					IsDeleted = false,
					Provincia = sede.Provincia,
					Telefono = sede.Telefono,
				};
				SediDTO toDelete = sedi.Find(x => x.SedeId == sede.SedeId);
				sedi.Remove(toDelete);
				sedi.Add(sede);
				File.WriteAllText("sedi.json", JsonConvert.SerializeObject(sedi));
				return new Response<bool>(true, 1, HttpStatusCode.OK, "Modificata con successo");
			}
			sede.SedeId = sedi.Max(x => x.SedeId);
			sede.IsDeleted = false;
			sedi.Add(sede);
			File.WriteAllText("sedi.json", JsonConvert.SerializeObject(sedi));
			return new Response<bool>(true, 1, HttpStatusCode.Created, "Aggiunta con successo");
		}
		public async Task<Response<bool>> UpdateSede(SediDTO sede)
		{
			string json = File.ReadAllText("sedi.json");
			List<SediDTO> sedi = JsonConvert.DeserializeObject<List<SediDTO>>(json);
			if (sedi.Any(x => x.SedeId == sede.SedeId && x.IsDeleted == false) == false)
				return new Response<bool>(false, 1, HttpStatusCode.NotFound, $"Nessuna sede con Id = {sede.SedeId}");
			sede.IsDeleted = false;
			SediDTO toDelete = sedi.Find(x => x.SedeId == sede.SedeId);
			sedi.Remove(toDelete);
			sedi.Add(sede);
			File.WriteAllText("sedi.json", JsonConvert.SerializeObject(sedi));
			return new Response<bool>(true, 1, HttpStatusCode.OK, "Modificato con successo");
		}
		public async Task<Response<bool>> DeactivateSede(int id)
		{
			string json = File.ReadAllText("sedi.json");
			List<SediDTO> sedi = JsonConvert.DeserializeObject<List<SediDTO>>(json);
			if (sedi.Any(x => x.SedeId == id && x.IsDeleted == false) == false)
				return new Response<bool>(false, 1, HttpStatusCode.NotFound, $"Nessuna sede con Id = {id}");
			SediDTO sede = sedi.Find(x => x.SedeId == id);
			sede.IsDeleted = false;
			SediDTO toDelete = sedi.Find(x => x.SedeId == sede.SedeId);
			sedi.Remove(toDelete);
			sedi.Add(sede);
			File.WriteAllText("sedi.json", JsonConvert.SerializeObject(sedi));
			return new Response<bool>(true, 1, HttpStatusCode.OK, "Disattivato con successo");
		}
		#endregion
	}
}
