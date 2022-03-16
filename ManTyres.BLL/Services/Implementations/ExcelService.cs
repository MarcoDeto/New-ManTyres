using AutoMapper;
using ManTyres.BLL.Repository.Interfaces;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Implementations
{
	public class ExcelService : IExcelService
	{
		private readonly IExcelRepository _repository;
		private readonly IClientiService _clientiService;
		private readonly IVeicoliService _veicoliService;
		private readonly IPneumaticiService _pneumaticiService;
		private readonly IInventarioService _inventarioService;
		private readonly IMapper _mapper;
		public ExcelService(IExcelRepository repository, IClientiService clientiService, IVeicoliService veicoliService,
			 IPneumaticiService pneumaticiService, IInventarioService inventarioService, IMapper mapper)
		{
			_repository = repository;
			_clientiService = clientiService;
			_veicoliService = veicoliService;
			_pneumaticiService = pneumaticiService;
			_inventarioService = inventarioService;
			_mapper = mapper;
		}

		#region METHODS
		public Response<byte[]> TracciatoClienti()
		{
			var result = _repository.ExportClienti(null);
			return new Response<byte[]>(result, 1, HttpStatusCode.OK, null);
		}
		public Response<byte[]> TracciatoVeicoli()
		{
			var result = _repository.ExportVeicoli(null);
			return new Response<byte[]>(result, 1, HttpStatusCode.OK, null);
		}
		public Response<byte[]> TracciatoAll()
		{
			var result = _repository.ExportPneumatici(null);
			return new Response<byte[]>(result, 1, HttpStatusCode.OK, null);
		}
		public async Task<Response<bool>> ImportClienti(Stream file)
		{
			var dt = _repository.ImportExcel(file);
			List<ClientiDTO> clienti = new List<ClientiDTO>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				ClientiDTO cliente = new ClientiDTO();
				cliente.ClienteId = 0;
				cliente.IsAzienda = dt.Rows[i]["Azienda"].ToString() == true.ToString() ? true : false;
				cliente.Nome = dt.Rows[i]["Nome"].ToString();
				cliente.Cognome = dt.Rows[i]["Cognome"].ToString();
				cliente.CodiceFiscale = dt.Rows[i]["Codice fiscale"].ToString();
				cliente.PartitaIva = dt.Rows[i]["Partita IVA"].ToString();
				cliente.Email = dt.Rows[i]["Email"].ToString();
				cliente.Telefono = dt.Rows[i]["Telefono"].ToString();
				cliente.Indirizzo = dt.Rows[i]["Indirizzo"].ToString();
				cliente.Civico = dt.Rows[i]["Civico"].ToString();
				cliente.Comune = dt.Rows[i]["Comune"].ToString();
				cliente.Cap = dt.Rows[i]["Cap"].ToString();
				cliente.Provincia = dt.Rows[i]["Provincia"].ToString();
				cliente.Nazione = dt.Rows[i]["Nazione"].ToString();
				cliente.IsDeleted = false;
				cliente.DataCreazione = DateTime.Now;
				clienti.Add(cliente);
			}
			return await _clientiService.AddList(clienti);
		}
		public async Task<Response<bool>> ImportVeicoli(Stream file)
		{
			var dt = _repository.ImportExcel(file);
			List<VeicoliDTO> veicoli = new List<VeicoliDTO>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				VeicoliDTO veicolo = new VeicoliDTO();
				veicolo.Marca = dt.Rows[i]["Marca"].ToString();
				veicolo.Modello = dt.Rows[i]["Modello"].ToString();
				veicolo.Targa = dt.Rows[i]["Targa"].ToString();
				veicolo.ClienteId = null;
				veicolo.IsDeleted = false;
				veicolo.DataCreazione = DateTime.Now;
				veicoli.Add(veicolo);
			}
			return await _veicoliService.AddList(veicoli);
		}
		public async Task<Response<bool>> ImportAll(Stream file, int sedeId, string userId, string ci)
		{
			CultureInfo cultureInfo = new CultureInfo(ci);
			var dt = _repository.ImportExcel(file);
			List<InventarioDTO> list = new List<InventarioDTO>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				var cliente = new ClientiDTO();
				if (dt.Columns["CLIENTE ID"] != null)
				{
					int.TryParse(dt.Rows[i]["CLIENTE ID"].ToString(), out int clienteID);
					cliente.ClienteId = clienteID;
				}
				string nomeCliente = null;
				if (dt.Columns["CLIENTE"] != null)
				{
					nomeCliente = dt.Rows[i]["CLIENTE"].ToString().TrimEnd();
					if (string.IsNullOrWhiteSpace(nomeCliente))
					{ continue; }
				}
				//else { continue; }
				var nomeSplitted = nomeCliente.Split(' ');
				for (var n = 0; n < nomeSplitted.Length; n++)
				{
					nomeSplitted[n] = char.ToUpper(nomeSplitted[n][0]) + nomeSplitted[n].Substring(1).ToLower();
					cliente.Nome += nomeSplitted[n] + " ";
				}
				cliente.Nome.TrimEnd();
				var IsAzienda = dt.Rows[i]["AZIENDA"].ToString();
				if (dt.Columns["AZIENDA"].ToString() != null)
					cliente.IsAzienda = dt.Rows[i]["AZIENDA"].ToString().ToLower() == "X" ? true : false;

				//"dd-MM-yyyy"
				InventarioDTO inventario = new InventarioDTO();
				DateTime.TryParseExact(dt.Rows[i]["INIZIO DEPOSITO"].ToString(), "dd/MM/yyyy hh:mm:ss", cultureInfo, DateTimeStyles.None, out DateTime inizioDeposito);
				if (DateTime.Compare(DateTime.ParseExact("01/01/0001 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), inizioDeposito) == 0)
				{
					continue;
				}
				inventario.InizioDeposito = inizioDeposito.AddHours(8);
				DateTime.TryParseExact(dt.Rows[i]["FINE DEPOSITO"].ToString(), "dd/MM/yyyy hh:mm:ss", cultureInfo, DateTimeStyles.None, out DateTime fineDeposito);
				inventario.FineDeposito = DateTime.Compare(DateTime.ParseExact("01/01/0001 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), fineDeposito) == 0
					 ? null
					 : fineDeposito.AddHours(8);

				var sede = new SediDTO();
				if (dt.Columns["SEDE"] != null)
					sede.Comune = dt.Rows[i]["SEDE"].ToString();

				var deposito = new DepositiDTO();

				deposito.Sede = sede;
				if (string.IsNullOrWhiteSpace(deposito.Sede.Comune))
				{
					if (sedeId == 0) { continue; }
					deposito.SedeId = sedeId;
				}

				if (dt.Columns["UBICAZIONE"] != null)
					deposito.Ubicazione = dt.Rows[i]["UBICAZIONE"].ToString().TrimEnd();

				var stagione = new StagioniDTO();
				if (dt.Columns["STAGIONE"] != null)
					stagione.Nome = dt.Rows[i]["STAGIONE"].ToString();

				var veicolo = new VeicoliDTO();
				if (dt.Columns["TARGA"] != null)
					veicolo.Targa = dt.Rows[i]["TARGA"].ToString().TrimEnd().ToUpper();
				else { continue; }
				veicolo.Cliente = cliente;

				var pneumatici = new PneumaticiDTO();
				if (dt.Columns["MARCA"] != null)
					pneumatici.Marca = dt.Rows[i]["MARCA"].ToString().TrimEnd();
				if (dt.Columns["MODELLO"] != null)
					pneumatici.Modello = dt.Rows[i]["MODELLO"].ToString().TrimEnd();
				if (dt.Columns["Q.TA'"] != null)
				{
					int.TryParse(dt.Rows[i]["Q.TA'"].ToString(), out int quantità);
					pneumatici.Quantita = quantità;
				}
				if (dt.Columns["MISURA"] != null)
					pneumatici.Misura = dt.Rows[i]["MISURA"].ToString().TrimEnd();
				if (dt.Columns["DOT"] != null)
					pneumatici.Dot = dt.Rows[i]["DOT"].ToString().TrimEnd();
				pneumatici.Veicolo = veicolo;
				pneumatici.Stagione = stagione;

				var operatore = new UserDTO();
				if (dt.Columns["OPERATORE"] != null)
					operatore.UserName = dt.Rows[i]["OPERATORE"].ToString().TrimEnd();
				inventario.User = operatore;
				inventario.UserId = userId;
				if (dt.Columns["NOTE"] != null)
					inventario.StatoGomme = dt.Rows[i]["NOTE"].ToString().TrimEnd();
				inventario.Deposito = deposito;
				inventario.Deposito.SedeId = sedeId;
				inventario.Pneumatici = pneumatici;
				if (dt.Columns["BATTISTRADA"] != null)
				{
					string battistrada = dt.Rows[i]["BATTISTRADA"].ToString();
					int.TryParse(battistrada.Substring(0, battistrada.Count() - 3), out int battistradaResult);
					inventario.Battistrada = battistradaResult;
				}
				list.Add(inventario);
			}
			return await _pneumaticiService.AddList(list/*.OrderBy(x => x.InizioDeposito).ToList()*/);
		}
		public async Task<Response<byte[]>> ExportClienti()
		{
			var clienti = await _clientiService.GetAll();
			var result = _repository.ExportClienti(clienti.Content);
			return new Response<byte[]>(result, clienti.Count, HttpStatusCode.OK, null);
		}
		public async Task<Response<byte[]>> ExportVeicoli()
		{
			var veicoli = await _veicoliService.GetAll();
			var result = _repository.ExportVeicoli(veicoli.Content);
			return new Response<byte[]>(result, veicoli.Count, HttpStatusCode.OK, null);
		}
		public async Task<Response<byte[]>> ExportPneumatici(int sede, int stagione, bool orderByDesc)
		{
			var pneumatici = await _inventarioService.GetAllPneumatici(sede, stagione, orderByDesc);
			var result = _repository.ExportPneumatici(pneumatici.Content);
			return new Response<byte[]>(result, pneumatici.Count, HttpStatusCode.OK, null);
		}
		public async Task<Response<byte[]>> ExportInventario(int sede, int stagione, bool orderByDesc)
		{
			var pneumatici = await _inventarioService.GetAllInventario(sede, stagione, orderByDesc);
			var result = _repository.ExportPneumatici(pneumatici.Content);
			return new Response<byte[]>(result, pneumatici.Count, HttpStatusCode.OK, null);
		}
		public async Task<Response<byte[]>> ExportArchivio(int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc)
		{
			var pneumatici = await _inventarioService.GetAllArchivio(sede, stagione, inizioOrderByDesc, fineOrderByDesc);
			var result = _repository.ExportPneumatici(pneumatici.Content);
			return new Response<byte[]>(result, pneumatici.Count, HttpStatusCode.OK, null);
		}
		public async Task<Response<byte[]>> ExportCestino(int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc)
		{
			var pneumatici = await _inventarioService.GetAllCestino(sede, stagione, inizioOrderByDesc, fineOrderByDesc);
			var result = _repository.ExportPneumatici(pneumatici.Content);
			return new Response<byte[]>(result, pneumatici.Count, HttpStatusCode.OK, null);
		}
		#endregion


		#region UTILITIES
		private DateTime ReturnDate(string date)
		{
			var dateSplitted = date.Split('-');
			if (dateSplitted.Length == 0) { return DateTime.Now; }
			int.TryParse(dateSplitted[0], out int day);
			int month = DateTime.ParseExact(dateSplitted[1], "MMM", CultureInfo.CurrentCulture).Month;
			int.TryParse(dateSplitted[2], out int year);
			return new DateTime(year, month, day);
		}
		#endregion
	}
}
