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
	public class ClientiService : IClientiService
	{
		public ClientiService() { }

		#region METHODS
		public async Task<Response<ClientiDTO>> GetById(int id)
		{
			var checkId = await CheckId(id);
			if (checkId != null)
				return new Response<ClientiDTO>(null, 0, checkId.Code, checkId.Message);
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			var result = clienti.Find(x => x.ClienteId == id);
			return new Response<ClientiDTO>(result, 1, HttpStatusCode.OK, null);
		}
		public async Task<Response<List<ClientiDTO>>> GetAll()
		{
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> result = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			return Response(result);
		}
		public async Task<Response<List<ClientiDTO>>> GetAll(int skip, int take, bool orderByName, string filter = null)
		{
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> result = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			if (string.IsNullOrWhiteSpace(filter))
			{
				result = result.Where(x => x.IsDeleted == false).OrderBy(x => x.Nome).Skip(skip).Take(take).ToList();
				return Response(result);
			}

			if (orderByName == true)
				result = result.Where(x => x.IsDeleted == false && (x.Nome.ToLower().Contains(filter.ToLower())
							|| x.CodiceFiscale.ToLower().Contains(filter.ToLower()) ||
							x.PartitaIva.ToLower().Contains(filter.ToLower()))).OrderBy(x => x.Nome).Skip(skip).Take(take).ToList();
			else
				result = result.Where(x => x.IsDeleted == false && (x.Nome.ToLower().Contains(filter.ToLower())
							|| x.CodiceFiscale.ToLower().Contains(filter.ToLower()) ||
							x.PartitaIva.ToLower().Contains(filter.ToLower()))).OrderBy(x => x.DataCreazione).Skip(skip).Take(take).ToList();
			return Response(result);
		}
		public async Task<Response<bool>> Add(ClientiDTO data)
		{
			if (await IsAlreadyExists(data) != null)
				return await IsAlreadyExists(data);
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			ClientiDTO cliente = new ClientiDTO()
			{
				ClienteId = clienti.Max(x => x.ClienteId) + 1,
				Cap = data.Cap,
				Civico = data.Civico,
				CodiceFiscale = data.CodiceFiscale,
				Cognome = data.Cognome,
				Comune = data.Comune,
				DataCreazione = DateTime.Now,
				Email = data.Email,
				Indirizzo = data.Indirizzo,
				IsAzienda = data.IsAzienda,
				IsDeleted = false,
				Nazione = data.Nazione,
				Nome = data.Nome,
				PartitaIva = data.PartitaIva,
				Provincia = data.PartitaIva,
				Telefono = data.Telefono
			};
			clienti.Add(cliente);
			File.WriteAllText("clienti.json", JsonConvert.SerializeObject(clienti));
			return new Response<bool>(true, 1, HttpStatusCode.Created, "Aggiunto con successo");
		}
		public async Task<Response<bool>> AddList(List<ClientiDTO> data)
		{
			if (data.Count == 0)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Lista vuota");
			foreach (var cliente in data)
			{
				if (await IsAlreadyExists(cliente) != null)
					data.Remove(cliente);
			}
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			clienti.AddRange(data);
			File.WriteAllText("clienti.json", JsonConvert.SerializeObject(clienti));
			return new Response<bool>(true, data.Count, HttpStatusCode.Created, "Aggiunti con successo");
		}
		public async Task<Response<bool>> Update(ClientiDTO data)
		{
			if (await CheckId(data.ClienteId) != null)
				return await CheckId(data.ClienteId);
			if (await IsAlreadyExists(data) != null)
				return await IsAlreadyExists(data);
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			ClientiDTO toDelete = clienti.Find(x => x.ClienteId == data.ClienteId);
			clienti.Remove(toDelete);
			clienti.Add(data);
			File.WriteAllText("clienti.json", JsonConvert.SerializeObject(clienti));
			return new Response<bool>(true, 1, HttpStatusCode.OK, "Modificato con successo");
		}
		/*public async Task<Response<bool>> Deactivate(int id)
		{
			 if (await CheckId(id) != null)
				  return await CheckId(id);
			 var data = clienti.Find(x => x.ClienteId);
			 data.IsDeleted = true;
			 _context.Clienti.Update(data);
			 return await SaveChanges("Disattivato con successo");
		}
		public async Task<Response<bool>> Reactivate(int id)
		{
			 if (await CheckId(id) != null)
				  return await CheckId(id);
			 var data = clienti.Find(x => x.ClienteId);
			 data.IsDeleted = false;
			 _context.Clienti.Update(data);
			 return await SaveChanges("Disattivato con successo");
		}*/
		#endregion

		#region PrivateMethods
		private async Task<Response<bool>> CheckId(int id)
		{
			if (id <= 0)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Id non è valido!");
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			if (clienti.Any(x => x.ClienteId == id) == false)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, $"Nessuno con Id = {id}");
			return null;
		}
		private async Task<Response<bool>> IsAlreadyExists(ClientiDTO cliente)
		{
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			if (string.IsNullOrWhiteSpace(cliente.CodiceFiscale) == false &&
				 clienti.Any(x => x.ClienteId != cliente.ClienteId
				 && x.CodiceFiscale == cliente.CodiceFiscale))
				new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Codice fiscale già esistente!");
			else if (string.IsNullOrWhiteSpace(cliente.PartitaIva) == false &&
				 clienti.Any(x => x.ClienteId != cliente.ClienteId
				 && x.PartitaIva == cliente.PartitaIva))
				new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Partita IVA già esistente!");
			return null;
		}
		private Response<List<ClientiDTO>> Response(List<ClientiDTO> data)
		{
			if (data.Count == 0)
				return new Response<List<ClientiDTO>>(null, 0, HttpStatusCode.OK, "Nessun cliente trovato");
			return new Response<List<ClientiDTO>>(data, data.Count, HttpStatusCode.OK, null);
		}
		#endregion
	}
}
