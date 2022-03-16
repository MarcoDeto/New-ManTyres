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
	public class PneumaticiService : IPneumaticiService
	{
		public PneumaticiService() { }

		#region METHODS
		public async Task<Response<List<InventarioDTO>>> GetLast2()
		{
			string json = File.ReadAllText("inventario.json");
			List<InventarioDTO> collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			var result = new List<InventarioDTO>();
			var pneumatici = collection.Where(x => x.Pneumatici.IsDeleted == false)
								 .OrderByDescending(x => x.InizioDeposito).Select(x => x.PneumaticiId).Distinct().Take(2).ToList();
			for (var i = 0; i < 2; i++)
			{
				if (pneumatici.Count > i)
				{
					var last = collection.Where(x => x.PneumaticiId == pneumatici[i]).OrderByDescending(t => t.InizioDeposito).First();
					var inventario = collection.FirstOrDefault(x => x.PneumaticiId == pneumatici[i] && x.InizioDeposito == last.InizioDeposito);
					result.Add(inventario);
				}
			}
			return new Response<List<InventarioDTO>>(result, 2, HttpStatusCode.OK, null);
		}
		public async Task<Response<List<InventarioDTO>>> GetByTarga(string targa)
		{
			string json = File.ReadAllText("inventario.json");
			List<InventarioDTO> collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			var result = new List<InventarioDTO>();
			var pneumatici = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa)
								 .Select(c => c.PneumaticiId).Distinct().ToList();
			foreach (var item in pneumatici)
			{
				var last = collection.Where(x => x.PneumaticiId == item).OrderByDescending(t => t.InizioDeposito).First();
				var inventario = collection.FirstOrDefault(x => x.PneumaticiId == item && x.InizioDeposito == last.InizioDeposito);
				result.Add(inventario);
			}
			return new Response<List<InventarioDTO>>(result, pneumatici.Count, HttpStatusCode.OK, null);
		}
		public async Task<Response<bool>> Add(InventarioDTO entity)
		{
			string json = File.ReadAllText("inventario.json");
			List<InventarioDTO> collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			if (await Check(entity) != null)
				return await Check(entity);
			DepositiDTO _deposito = await checkUbicazione(entity);
			if (_deposito == null)
				return new Response<bool>(false, 0, HttpStatusCode.InternalServerError, "Deposito non salvato");

			json = File.ReadAllText("stagioni.json");
			List<StagioniDTO> stagioni = JsonConvert.DeserializeObject<List<StagioniDTO>>(json);
			json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			json = File.ReadAllText("pneumatici.json");
			List<PneumaticiDTO> pneumaticiList = JsonConvert.DeserializeObject<List<PneumaticiDTO>>(json);
			var pneumatici = new PneumaticiDTO()
			{
				Marca = entity.Pneumatici.Marca,
				Modello = entity.Pneumatici.Modello,
				Misura = entity.Pneumatici.Misura,
				Dot = entity.Pneumatici.Dot,
				StagioneId = entity.Pneumatici.StagioneId,
				VeicoloId = entity.Pneumatici.VeicoloId,
				DataUbicazione = DateTime.Now,
				Quantita = entity.Pneumatici.Quantita,
				IsDeleted = false,
				PneumaticiId = pneumaticiList.Max(x => x.PneumaticiId),
				Stagione = stagioni.Find(x => x.StagioneId == entity.Pneumatici.StagioneId),
				Veicolo = veicoli.Find(x => x.VeicoloId == entity.Pneumatici.VeicoloId)
			};
			pneumaticiList.Add(pneumatici);
			File.WriteAllText("pneumatici.json", JsonConvert.SerializeObject(pneumaticiList));
			json = File.ReadAllText("users.json");
			List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
			var inventario = new InventarioDTO()
			{
				InizioDeposito = entity.InizioDeposito,
				DepositoId = _deposito.DepositoId,
				Battistrada = entity.Battistrada,
				StatoGomme = entity.StatoGomme,
				UserId = entity.UserId,
				Deposito = _deposito,
				PneumaticiId = pneumatici.PneumaticiId,
				FineDeposito = null,
				IsDeleted = false,
				Pneumatici = pneumatici,
				//User = users.Find(x => x.Id == entity.UserId),
			};
			InventarioDTO checkInventario = collection.SingleOrDefault(x => x.FineDeposito == null && x.PneumaticiId == inventario.PneumaticiId);
			if (checkInventario != null)
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Pneumatici già in deposito");
			collection.Add(inventario);
			return new Response<bool>(true, 1, HttpStatusCode.Created, "Pneumatici aggiunti in deposito");
		}
		public async Task<Response<bool>> AddList(List<InventarioDTO> archivio)
		{
			if (archivio.Count == 0)
				return new Response<bool>(false, 0, HttpStatusCode.InternalServerError, "Prova a selezionare la sede");
			var count = 0;
			string json = File.ReadAllText("inventario.json");
			List<InventarioDTO> collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			json = File.ReadAllText("users.json");
			List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
			foreach (var row in archivio)
			{
				if (collection.Find(x => x.PneumaticiId == row.PneumaticiId && x.InizioDeposito == row.InizioDeposito) != null)
					continue;
				if (collection.Any(x => x.FineDeposito == null && x.PneumaticiId == row.PneumaticiId))
					continue;
				DepositiDTO deposito = await checkUbicazione(row);
				if (deposito == null) { continue; }
				PneumaticiDTO pneumatici = await checkPneumatici(row);
				if (pneumatici == null) { continue; }
				UserDTO user = new UserDTO();
				if (string.IsNullOrWhiteSpace(row.User.UserName) == false)
					user = users.SingleOrDefault(x => x.UserName == row.User.UserName);
				row.User = user;
			//	if (users.Any(x => x.Id == user.Id) == false) { row.UserId = user.Id; }
				row.Pneumatici = pneumatici;
				row.PneumaticiId = pneumatici.PneumaticiId;
				row.Deposito = deposito;
				row.DepositoId = deposito.DepositoId;
				row.IsDeleted = false;
				collection.Add(row);
				count++;
			}
			File.WriteAllText("inventario.json", JsonConvert.SerializeObject(collection));
			return new Response<bool>(true, count, HttpStatusCode.Created, "Pneumatici aggiunti in deposito");
		}
		public async Task<Response<bool>> GenerateData()
		{
			SediDTO[] sedi = { new SediDTO { SedeId = 1, Cap = "20100", Civico = "1", Indirizzo = "Via", Comune = "MIlano", IsDeleted = false, Nazione = "Italia", Provincia = "MIlano", Telefono = "+39 3515737641" } };
			File.WriteAllText("sedi.json", JsonConvert.SerializeObject(sedi));
			//UserDTO[] users = { new UserDTO { FirstName = "Marco", LastName = "De Tomasi", Email = "marco.detomasi@yahoo.it", EmailConfirmed = true, Id = "1", ImgProfile = null, IsDeleted = false, PasswordHash = "Admin000!", PhoneNumber = "+39 3515737641", Role = " admin", UserName = "Admin" } };
			//File.WriteAllText("users.json", JsonConvert.SerializeObject(users));
			StagioniDTO[] stagioni = { new StagioniDTO { Nome = "estate", StagioneId = 1 }, new StagioniDTO { Nome = "inverno", StagioneId = 2 } };
			File.WriteAllText("stagioni.json", JsonConvert.SerializeObject(stagioni));

			List<DepositiDTO> depositi = new List<DepositiDTO>();
			List<ClientiDTO> clienti = new List<ClientiDTO>();
			List<VeicoliDTO> veicoli = new List<VeicoliDTO>();
			List<PneumaticiDTO> pneumaticiList = new List<PneumaticiDTO>();
			List<InventarioDTO> result = new List<InventarioDTO>();
			var rand = new Random();
			string[] marche = { "Ford", "Audi", "BMW", "Fiat", "Lancia", "Toyota", "Jeep", "Tesla" };
			string[] modelli = { "Fiesta", "Q5", "X1", "Panda", "Ypsilon", "Yaris", "Renegade", "Model 3" };
			string[] marchePneu = { "Michelin", "Pirelli", "Continental", "Nokian", "Goodyear", "General", "Hankook", "Dunlop" };
			var count = 0;
			for (var i = 300; i > 0; i--)
			{
				count++;
				DepositiDTO deposito = new DepositiDTO()
				{
					SedeId = 1,
					Ubicazione = "COL " + i.ToString("000"),
					DepositoId = count,
					IsDeleted = false,
					Sede = sedi.FirstOrDefault(x => x.SedeId == 1)
				};
				depositi.Add(deposito);

				var isPrivatoOrAzienda = rand.Next(0, 1) == 0 ? true : false;
				ClientiDTO cliente = new ClientiDTO()
				{
					Nome = "CLIENTE " + i.ToString("000"),
					DataCreazione = DateTime.Now.AddDays(-i + 1),
					CodiceFiscale = isPrivatoOrAzienda ? "CLNCLN00A00A" + count.ToString("000") + "A" : null,
					Cap = "20100",
					Comune = "Milano",
					Indirizzo = "Via",
					Email = "cliente@email.com",
					Telefono = "+39 333 444 5555",
					IsAzienda = false,
					Nazione = "Italia",
					Provincia = "Milano",
					Civico = "0",
					ClienteId = count,
					Cognome = null,
					IsDeleted = false,
					PartitaIva = isPrivatoOrAzienda ? null : "86334519" + count.ToString("000")
				};
				clienti.Add(cliente);

				int RandomIndex = rand.Next(marchePneu.Length);
				VeicoliDTO veicolo = new VeicoliDTO()
				{
					Cliente = cliente,
					ClienteId = cliente.ClienteId,
					IsDeleted = false,
					Targa = "AA" + count.ToString("000") + "ZZ",
					Marca = marche[RandomIndex],
					Modello = modelli[RandomIndex],
					VeicoloId = count,
					DataCreazione = DateTime.Now.AddDays(-i + 1)
				};
				veicoli.Add(veicolo);

				var stagioneId = isPrivatoOrAzienda ? 1 : 2;
				PneumaticiDTO pneumatici = new PneumaticiDTO()
				{
					Marca = marchePneu[RandomIndex],
					Misura = "205/55 R16 91H",
					DataUbicazione = DateTime.Now.AddDays(-i + 1),
					Quantita = 4,
					Stagione = stagioni.FirstOrDefault(x => x.StagioneId == stagioneId),
					StagioneId = stagioneId,
					Dot = "0" + count.ToString("000"),
					IsDeleted = false,
					Modello = null,
					PneumaticiId = count,
					Veicolo = veicolo,
					VeicoloId = veicolo.VeicoloId
				};
				pneumaticiList.Add(pneumatici);

				InventarioDTO inventario = new InventarioDTO()
				{
					Battistrada = rand.Next(1, 10),
					Deposito = deposito,
					DepositoId = deposito.DepositoId,
					FineDeposito = null,
					InizioDeposito = DateTime.Now.AddDays(-i + 1),
					IsDeleted = false,
					StatoGomme = "Buono",
					UserId = "1",
					//User = users.First(),
					PneumaticiId = pneumatici.PneumaticiId,
					Pneumatici = pneumatici
				};
				result.Add(inventario);
			}
			File.WriteAllText("depositi.json", JsonConvert.SerializeObject(depositi));
			File.WriteAllText("clienti.json", JsonConvert.SerializeObject(clienti));
			File.WriteAllText("veicoli.json", JsonConvert.SerializeObject(veicoli));
			File.WriteAllText("pneumatici.json", JsonConvert.SerializeObject(pneumaticiList));
			File.WriteAllText("inventario.json", JsonConvert.SerializeObject(result));
			return new Response<bool>(true, result.Count, HttpStatusCode.OK, null);
		}
		public async Task<Response<bool>> Update(InventarioDTO entity)
		{
			if (await Check(entity) != null)
				return await Check(entity);
			DepositiDTO _deposito = await checkUbicazione(entity);
			if (_deposito == null)
				return new Response<bool>(false, 0, HttpStatusCode.InternalServerError, "Deposito non salvato");
			string json = File.ReadAllText("pneumatici.json");
			List<PneumaticiDTO> pneumaticiList = JsonConvert.DeserializeObject<List<PneumaticiDTO>>(json);
			PneumaticiDTO _pneumatici = pneumaticiList.Find(x => x.PneumaticiId == entity.PneumaticiId);
			if (_pneumatici == null)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, "Pneumatici non trovati");
			json = File.ReadAllText("inventario.json");
			List<InventarioDTO> collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			InventarioDTO _inventario = collection.Find(x => x.PneumaticiId == entity.PneumaticiId && x.InizioDeposito == entity.InizioDeposito);
			if (_inventario == null)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, "Pneumatici non inseriti il " + entity.InizioDeposito);
			collection.Remove(_inventario);
			json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			json = File.ReadAllText("stagioni.json");
			List<StagioniDTO> stagioni = JsonConvert.DeserializeObject<List<StagioniDTO>>(json);
			_pneumatici.Veicolo = veicoli.Find(x => x.VeicoloId == entity.Pneumatici.VeicoloId);
			_pneumatici.VeicoloId = entity.Pneumatici.VeicoloId;
			_pneumatici.Stagione = stagioni.Find(x => x.StagioneId == entity.Pneumatici.StagioneId);
			_pneumatici.StagioneId = entity.Pneumatici.StagioneId;
			_pneumatici.Marca = entity.Pneumatici.Marca;
			_pneumatici.Modello = entity.Pneumatici.Modello;
			_pneumatici.Misura = entity.Pneumatici.Misura;
			_pneumatici.Dot = entity.Pneumatici.Dot;
			_pneumatici.Quantita = entity.Pneumatici.Quantita;
			_pneumatici.IsDeleted = false;
			pneumaticiList.Remove(entity.Pneumatici);
			pneumaticiList.Add(entity.Pneumatici);
			File.WriteAllText("pneumatici.json", JsonConvert.SerializeObject(pneumaticiList));

			json = File.ReadAllText("users.json");
			List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
			json = File.ReadAllText("depositi.json");
			List<DepositiDTO> depositi = JsonConvert.DeserializeObject<List<DepositiDTO>>(json);
			_inventario.UserId = entity.UserId;
			//_inventario.User = users.Find(x => x.Id == entity.UserId);
			_inventario.DepositoId = _deposito.DepositoId;
			_inventario.Deposito = depositi.Find(x => x.DepositoId == entity.DepositoId);
			_inventario.Battistrada = entity.Battistrada;
			_inventario.StatoGomme = entity.StatoGomme;
			_inventario.Pneumatici = _pneumatici;
			_inventario.PneumaticiId = _pneumatici.PneumaticiId;
			collection.Add(_inventario);
			File.WriteAllText("inventario.json", JsonConvert.SerializeObject(collection));
			return new Response<bool>(true, 1, HttpStatusCode.OK, "Pneumatici e deposito aggiornati");
		}
		public async Task<Response<bool>> FineDeposito(InventarioDTO entity)
		{
			string json = File.ReadAllText("inventario.json");
			List<InventarioDTO> collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			InventarioDTO _inventario = collection.Find(x => x.PneumaticiId == entity.PneumaticiId && x.InizioDeposito == entity.InizioDeposito);
			if (_inventario == null)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, "Pneumatici non trovati");
			collection.Remove(_inventario);
			_inventario.FineDeposito = entity.FineDeposito;
			json = File.ReadAllText("users.json");
			List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
			_inventario.UserId = entity.UserId;
			//_inventario.User = users.Find(x => x.Id == entity.UserId);
			collection.Add(_inventario);
			File.WriteAllText("inventario.json", JsonConvert.SerializeObject(collection));
			return new Response<bool>(true, 1, HttpStatusCode.OK, "Deposito finito con successo");
		}
		public async Task<Response<bool>> InizioDeposito(InventarioDTO entity)
		{
			DepositiDTO _deposito = await checkUbicazione(entity);
			if (_deposito == null)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, "Deposito non salvato");
			string json = File.ReadAllText("inventario.json");
			List<InventarioDTO> collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			if (collection.Any(x => x.FineDeposito == null && x.PneumaticiId == entity.PneumaticiId))
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, "Pneumatici già depositati");
			json = File.ReadAllText("pneumatici.json");
			List<PneumaticiDTO> pneumaticiList = JsonConvert.DeserializeObject<List<PneumaticiDTO>>(json);
			PneumaticiDTO _pneumatici = pneumaticiList.Find(x => x.PneumaticiId == entity.PneumaticiId);
			json = File.ReadAllText("users.json");
			List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
			//UserDTO _user = users.Find(x => x.Id == entity.UserId);

			var _inventario = new InventarioDTO()
			{
				Pneumatici = _pneumatici,
				PneumaticiId = entity.PneumaticiId,
				InizioDeposito = entity.InizioDeposito,
				FineDeposito = null,
				Deposito = _deposito,
				DepositoId = _deposito.DepositoId,
				Battistrada = entity.Battistrada,
				StatoGomme = entity.StatoGomme,
				//User = _user,
				UserId = entity.UserId,
				IsDeleted = false
			};
			collection.Add(_inventario);
			File.WriteAllText("inventario.json", JsonConvert.SerializeObject(collection));
			return new Response<bool>(true, 1, HttpStatusCode.OK, "Deposito salvato con successo");
		}
		#endregion

		#region UTILITIES
		public async Task<Response<bool>> Check(InventarioDTO entity)
		{
			string json = File.ReadAllText("stagioni.json");
			List<StagioniDTO> stagioni = JsonConvert.DeserializeObject<List<StagioniDTO>>(json);
			if (stagioni.Any(x => x.StagioneId == entity.Pneumatici.StagioneId) == false)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, "Stagione non trovata");
			json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			if (veicoli.Any(x => x.VeicoloId == entity.Pneumatici.VeicoloId) == false)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, "Veicolo non trovato");
			json = File.ReadAllText("sedi.json");
			List<SediDTO> sedi = JsonConvert.DeserializeObject<List<SediDTO>>(json);
			if (sedi.Any(x => x.SedeId == entity.Deposito.SedeId) == false)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, "Sede non trovata");
			json = File.ReadAllText("users.json");
			List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
			//if (users.Any(x => x.Id == entity.UserId) == false)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, "Utente non trovato");
			return null;
		}
		private async Task<StagioniDTO> checkStagione(InventarioDTO entity)
		{
			string json = File.ReadAllText("stagioni.json");
			List<StagioniDTO> stagioni = JsonConvert.DeserializeObject<List<StagioniDTO>>(json);

			if (entity.Pneumatici.StagioneId != 0)
			{
				var test = stagioni.Find(x => x.StagioneId == entity.Pneumatici.StagioneId);
				return test;
			}

			string nomeStagione = entity.Pneumatici.Stagione.Nome;
			if (nomeStagione.Length > 0)
			{
				nomeStagione = nomeStagione.Substring(0, 1).ToLower();
				if (stagioni.Any(x => x.Nome.StartsWith(nomeStagione)))
					return stagioni.SingleOrDefault(x => x.Nome.StartsWith(nomeStagione));
			}
			return null;
		}
		private async Task<ClientiDTO> checkCliente(InventarioDTO entity)
		{
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);

			ClientiDTO cliente = null;
			if (entity.Pneumatici.Veicolo.Cliente.ClienteId != 0)
				cliente = clienti.Find(x => x.ClienteId == entity.Pneumatici.Veicolo.Cliente.ClienteId);
			if (cliente == null)
				cliente = clienti.SingleOrDefault(x => x.Nome == entity.Pneumatici.Veicolo.Cliente.Nome);
			if (cliente == null)
			{
				cliente = new ClientiDTO()
				{
					ClienteId = clienti.Max(x => x.ClienteId) + 1,
					Cap = entity.Pneumatici.Veicolo.Cliente.Cap,
					Civico = entity.Pneumatici.Veicolo.Cliente.Civico,
					CodiceFiscale = entity.Pneumatici.Veicolo.Cliente.CodiceFiscale,
					Cognome = entity.Pneumatici.Veicolo.Cliente.Cognome,
					Comune = entity.Pneumatici.Veicolo.Cliente.Comune,
					DataCreazione = entity.InizioDeposito,
					Email = entity.Pneumatici.Veicolo.Cliente.Email,
					Indirizzo = entity.Pneumatici.Veicolo.Cliente.Indirizzo,
					IsAzienda = entity.Pneumatici.Veicolo.Cliente.IsAzienda,
					IsDeleted = false,
					Nazione = entity.Pneumatici.Veicolo.Cliente.Nazione,
					Nome = entity.Pneumatici.Veicolo.Cliente.Nome,
					PartitaIva = entity.Pneumatici.Veicolo.Cliente.PartitaIva,
					Provincia = entity.Pneumatici.Veicolo.Cliente.PartitaIva,
					Telefono = entity.Pneumatici.Veicolo.Cliente.Telefono
				};
				clienti.Add(cliente);
			}
			File.WriteAllText("clienti.json", JsonConvert.SerializeObject(clienti));
			return cliente;
		}
		private async Task<VeicoliDTO> checkVeicolo(InventarioDTO entity)
		{
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);

			VeicoliDTO veicolo = null;
			if (entity.Pneumatici.Veicolo.VeicoloId != 0)
				veicolo = veicoli.Find(x => x.VeicoloId == entity.Pneumatici.Veicolo.VeicoloId);
			if (veicolo == null)
				veicolo = veicoli.SingleOrDefault(x => x.Targa == entity.Pneumatici.Veicolo.Targa);
			if (veicolo == null)
			{
				ClientiDTO cliente = await checkCliente(entity);
				veicolo = new VeicoliDTO()
				{
					Cliente = cliente,
					ClienteId = cliente.ClienteId,
					DataCreazione = entity.InizioDeposito,
					IsDeleted = false,
					Modello = entity.Pneumatici.Veicolo.Modello,
					Marca = entity.Pneumatici.Veicolo.Marca,
					Targa = entity.Pneumatici.Veicolo.Targa,
					VeicoloId = veicoli.Max(x => x.VeicoloId)
				};
				veicoli.Add(veicolo);
				File.WriteAllText("veicoli.json", JsonConvert.SerializeObject(veicoli));
			}
			return veicolo;
		}
		private async Task<PneumaticiDTO> checkPneumatici(InventarioDTO entity)
		{
			if (string.IsNullOrWhiteSpace(entity.Pneumatici.Veicolo.Targa)) { return null; }
			StagioniDTO stagione = await checkStagione(entity);
			if (stagione == null) { return null; }
			VeicoliDTO veicolo = await checkVeicolo(entity);
			if (veicolo == null) { return null; }
			string json = File.ReadAllText("pneumatici.json");
			List<PneumaticiDTO> pneumaticiList = JsonConvert.DeserializeObject<List<PneumaticiDTO>>(json);
			PneumaticiDTO pneumatici = pneumaticiList.SingleOrDefault(x => x.Veicolo.Targa == entity.Pneumatici.Veicolo.Targa &&
									  x.Marca == entity.Pneumatici.Marca && x.Misura == entity.Pneumatici.Misura);
			if (pneumatici != null)
				return pneumatici;
			pneumatici = new PneumaticiDTO()
			{
				DataUbicazione = entity.InizioDeposito,
				Dot = entity.Pneumatici.Dot,
				IsDeleted = false,
				Marca = entity.Pneumatici.Marca,
				Misura = entity.Pneumatici.Misura,
				Modello = entity.Pneumatici.Modello,
				PneumaticiId = pneumaticiList.Max(x => x.PneumaticiId),
				Quantita = entity.Pneumatici.Quantita,
				Stagione = stagione,
				StagioneId = stagione.StagioneId,
				Veicolo = veicolo,
				VeicoloId = veicolo.VeicoloId
			};
			pneumaticiList.Add(pneumatici);
			File.WriteAllText("pneumatici.json", JsonConvert.SerializeObject(pneumaticiList));
			return pneumatici;
		}
		private async Task<DepositiDTO> checkUbicazione(InventarioDTO entity)
		{
			if (entity.Deposito == null) { return null; }
			if (string.IsNullOrWhiteSpace(entity.Deposito.Ubicazione)) { return null; }

			string json = File.ReadAllText("sedi.json");
			List<SediDTO> sedi = JsonConvert.DeserializeObject<List<SediDTO>>(json);
			json = File.ReadAllText("depositi.json");
			List<DepositiDTO> depositi = JsonConvert.DeserializeObject<List<DepositiDTO>>(json);

			SediDTO _sede = sedi.Find(x => x.SedeId == entity.Deposito.SedeId);
			if (_sede == null) { _sede = sedi.SingleOrDefault(x => x.Comune == entity.Deposito.Sede.Comune); }
			if (_sede != null) { entity.Deposito.SedeId = _sede.SedeId; }

			DepositiDTO _deposito = depositi.SingleOrDefault(x => x.Ubicazione == entity.Deposito.Ubicazione && x.SedeId == entity.Deposito.SedeId);
			if (_deposito != null) { return _deposito; }

			DepositiDTO deposito = new DepositiDTO()
			{
				Ubicazione = entity.Deposito.Ubicazione,
				SedeId = entity.Deposito.SedeId,
				IsDeleted = false,
				Sede = _sede,
				DepositoId = depositi.Max(x => x.DepositoId) + 1
			};
			depositi.Add(deposito);
			File.WriteAllText("deposito.json", JsonConvert.SerializeObject(depositi));
			return deposito;
		}
		#endregion
	}
}
