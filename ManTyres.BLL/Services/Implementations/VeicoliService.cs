using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Implementations
{
	public class VeicoliService : IVeicoliService
	{
		public VeicoliService()
		{
		}

		#region METHODS
		public async Task<Response<VeicoliDTO>> GetById(int id)
		{
			var checkId = await CheckId(id);
			if (checkId != null)
				return new Response<VeicoliDTO>(null, 0, checkId.Code, checkId.Message);
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			var result = veicoli.Find(x => x.VeicoloId == id);
			return new Response<VeicoliDTO>(result, 1, HttpStatusCode.OK, null);
		}
		public async Task<Response<List<VeicoliDTO>>> GetByClienteId(int clienteId)
		{
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			var result = veicoli.Where(x => x.IsDeleted == false && x.ClienteId == clienteId)
							.OrderBy(x => x.Targa).ToList();
			return Response(result, result.Count);
		}
		public async Task<Response<List<VeicoliDTO>>> GetAllForSelectList()
		{
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			var result = veicoli.Where(x => x.IsDeleted == false).OrderBy(x => x.Targa).ToList();
			return Response(result, result.Count);
		}
		public async Task<Response<List<VeicoliDTO>>> GetAll()
		{
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			var result = veicoli.Where(x => x.IsDeleted == false).OrderBy(x => x.Targa).ToList();
			return Response(result, result.Count);
		}
		public async Task<Response<List<VeicoliDTO>>> GetAll(int skip, int take, bool orderByTarga, string filter = null)
		{
			var count = 0;
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			var result = new List<VeicoliDTO>();
			if (string.IsNullOrWhiteSpace(filter))
			{
				if (orderByTarga == true)
					result = veicoli.Where(x => x.IsDeleted == false)
							  .OrderBy(x => x.Targa).Skip(skip).Take(take).ToList();
				else
					result = veicoli.Where(x => x.IsDeleted == false)
							  .OrderBy(x => x.DataCreazione).Skip(skip).Take(take).ToList();
				count = veicoli.Where(x => x.IsDeleted == false).Count();
				return Response(result, count);
			}
			if (orderByTarga == true)
				result = veicoli.Where(x => x.IsDeleted == false &&
						  (x.Targa.StartsWith(filter.ToUpper()) || x.Cliente.Nome.ToLower().Contains(filter.ToLower())))
						  .OrderBy(x => x.Targa).Skip(skip).Take(take).ToList();
			else
				result = veicoli.Where(x => x.IsDeleted == false &&
						  (x.Targa.StartsWith(filter.ToUpper()) || x.Cliente.Nome.ToLower().Contains(filter.ToLower())))
						  .OrderBy(x => x.DataCreazione).Skip(skip).Take(take).ToList();
			count = veicoli.Where(x => x.IsDeleted == false &&
							(x.Targa.StartsWith(filter.ToUpper()) || x.Cliente.Nome.ToLower().Contains(filter.ToLower()))).Count();
			return Response(result, count);
		}
		public async Task<Response<List<VeicoliDTO>>> GetAllDeleted()
		{
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			var result = veicoli.Where(x => x.IsDeleted == true).OrderBy(x => x.Targa).ToList();
			return Response(result, result.Count);
		}
		public async Task<Response<bool>> Add(VeicoliDTO veicolo)
		{
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			if ((await IsAlreadyExists(veicolo)).Content == false)
				return await IsAlreadyExists(veicolo);
			var _veicolo = new VeicoliDTO()
			{
				Targa = veicolo.Targa,
				Marca = veicolo.Marca,
				Modello = veicolo.Modello,
				ClienteId = veicolo.ClienteId,
				IsDeleted = false,
				DataCreazione = DateTime.Now,
				VeicoloId = veicoli.Max(x => x.VeicoloId),
				Cliente = clienti.Find(x => x.ClienteId == veicolo.ClienteId)
			};
			veicoli.Add(_veicolo);
			File.WriteAllText("veicoli.json", JsonConvert.SerializeObject(veicoli));
			return new Response<bool>(true, 1, HttpStatusCode.Created, "Aggiunto con successo");
		}
		public async Task<Response<bool>> AddList(List<VeicoliDTO> data)
		{
			if (data.Count == 0)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Lista vuota");
			foreach (var veicolo in data)
			{
				if ((await IsAlreadyExists(veicolo)).Content)
					data.Remove(veicolo);
			}
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			veicoli.AddRange(data);
			File.WriteAllText("veicoli.json", JsonConvert.SerializeObject(veicoli));
			return new Response<bool>(true, data.Count, HttpStatusCode.Created, "Aggiunti con successo");
		}
		public async Task<Response<bool>> Update(VeicoliDTO veicolo)
		{
			if (await CheckId(veicolo.VeicoloId) != null)
				return await CheckId(veicolo.VeicoloId);
			if ((await IsAlreadyExists(veicolo)).Content == false)
				return await IsAlreadyExists(veicolo);
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			veicolo.Cliente = clienti.Find(x => x.ClienteId == veicolo.ClienteId);
			VeicoliDTO toDelete = veicoli.Find(x => x.VeicoloId == veicolo.VeicoloId);
			veicoli.Remove(toDelete);
			veicoli.Add(veicolo);
			File.WriteAllText("veicoli.json", JsonConvert.SerializeObject(veicoli));
			return new Response<bool>(true, 1, HttpStatusCode.OK, "Modificato con successo");
		}/*
        public async Task<Response<bool>> Deactivate(int id)
        {
            if (await CheckId(id) != null)
                return await CheckId(id);
            var data = veicoli.FindAsync(id);
            data.IsDeleted = true;
            veicoli.Update(data);
            return await SaveChanges("Disattivato con successo");
        }
        public async Task<Response<bool>> Reactivate(int id)
        {
            if (await CheckId(id) != null)
                return await CheckId(id);
            var data = veicoli.FindAsync(id);
            data.IsDeleted = false;
            veicoli.Update(data);
            return await SaveChanges("Disattivato con successo");
        }*/
		#endregion


		#region UTILITIES
		private async Task<Response<bool>> CheckId(int id)
		{
			if (id <= 0)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Id non è valido!");
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			if (veicoli.Any(x => x.VeicoloId == id) == false)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, $"Nessuno con Id = {id}");
			return null;
		}
		private async Task<Response<bool>> IsAlreadyExists(VeicoliDTO veicolo)
		{
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			if (clienti.Any(x => x.ClienteId == veicolo.ClienteId) == false)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Cliente non esistente!");
			if (veicoli.Any(x => x.VeicoloId != veicolo.VeicoloId && x.Targa == veicolo.Targa))
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Targa già esistente!");
			return new Response<bool>(true, 0, HttpStatusCode.OK, null);
		}
		private Response<List<VeicoliDTO>> Response(List<VeicoliDTO> data, int count)
		{
			if (count == 0)
				return new Response<List<VeicoliDTO>>(null, 0, HttpStatusCode.OK, "Nessun veicolo trovato");
			return new Response<List<VeicoliDTO>>(data, count, HttpStatusCode.OK, null);
		}
		#endregion
	}
}
