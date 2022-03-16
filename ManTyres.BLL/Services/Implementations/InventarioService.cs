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
	public class InventarioService : IInventarioService
	{
		public InventarioService() { }

		#region METHODS
		public async Task<Response<List<InventarioDTO>>> GetAllPneumatici(int sede, int stagione, bool orderByDesc)
		{
			string json = File.ReadAllText("inventario.json");
			var collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			var result = new List<InventarioDTO>();
			if (orderByDesc == true)
			{
				if (sede == 0 && stagione == 0)
					result = collection.OrderByDescending(x => x.InizioDeposito).ToList();
				else if (sede == 0)
					result = collection.Where(x => x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).ToList();
				else if (stagione == 0)
					result = collection.Where(x => x.Deposito.SedeId == sede).OrderByDescending(x => x.InizioDeposito).ToList();
				else
					result = collection.Where(x => x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).ToList();
			}
			else if (orderByDesc == false)
			{
				if (sede == 0 && stagione == 0)
					result = collection.OrderBy(x => x.InizioDeposito).ToList();
				else if (sede == 0)
					result = collection.Where(x => x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).ToList();
				else if (stagione == 0)
					result = collection.Where(x => x.Deposito.SedeId == sede).OrderBy(x => x.InizioDeposito).ToList();
				else
					result = collection.Where(x => x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).ToList();
			}
			return new Response<List<InventarioDTO>>(result, result.Count, HttpStatusCode.OK, null);
		}
		public async Task<Response<List<InventarioDTO>>> GetAllInventario(int sede, int stagione, bool orderByDesc)
		{
			string json = File.ReadAllText("inventario.json");
			var collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			var result = new List<InventarioDTO>(); if (orderByDesc == true)
			{
				if (sede == 0 && stagione == 0)
					result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false).OrderByDescending(x => x.InizioDeposito).ToList();
				else if (sede == 0)
					result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).ToList();
				else if (stagione == 0)
					result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderByDescending(x => x.InizioDeposito).ToList();
				else
					result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).ToList();
			}
			else if (orderByDesc == false)
			{
				if (sede == 0 && stagione == 0)
					result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false).OrderBy(x => x.InizioDeposito).ToList();
				else if (sede == 0)
					result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).ToList();
				else if (stagione == 0)
					result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderBy(x => x.InizioDeposito).ToList();
				else
					result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).ToList();
			}
			return new Response<List<InventarioDTO>>(result, result.Count, HttpStatusCode.OK, null);
		}
		public async Task<Response<List<InventarioDTO>>> GetInventario(int skip, int take, int sede, int stagione, bool orderByDesc, string targa = null)
		{
			string json = File.ReadAllText("inventario.json");
			var collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			var result = new List<InventarioDTO>();
			int count = 0;
			if (targa == null)
			{
				if (orderByDesc == true)
				{
					if (sede == 0 && stagione == 0)
					{
						result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false).Count();
					}
					else if (sede == 0)
					{
						result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).Count();
					}
					else if (stagione == 0)
					{
						result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede).Count();
					}
					else
					{
						result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
					}
				}
				else if (orderByDesc == false)
				{
					if (sede == 0 && stagione == 0)
					{
						result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false).Count();
					}
					else if (sede == 0)
					{
						result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).Count();
					}
					else if (stagione == 0)
					{
						result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede).Count();
					}
					else
					{
						result = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
					}
				}
			}
			else
			{
				if (orderByDesc == true)
				{
					if (sede == 0 && stagione == 0)
					{
						result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false).Count();
					}
					else if (sede == 0)
					{
						result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).Count();

					}
					else if (stagione == 0)
					{
						result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede).Count();
					}
					else
					{
						result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
					}
				}
				else if (orderByDesc == false)
				{
					if (sede == 0 && stagione == 0)
					{
						result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false).Count();
					}
					else if (sede == 0)
					{
						result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).Count();

					}
					else if (stagione == 0)
					{
						result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede).Count();
					}
					else
					{
						result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
						count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito == null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
					}
				}
			}
			return new Response<List<InventarioDTO>>(result, count, HttpStatusCode.OK, null);
		}
		public async Task<Response<List<InventarioDTO>>> GetAllArchivio(int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc)
		{
			string json = File.ReadAllText("inventario.json");
			var collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			var result = new List<InventarioDTO>();
			if (inizioOrderByDesc != null)
			{
				if (inizioOrderByDesc == true)
				{
					if (sede == 0 && stagione == 0)
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false).OrderByDescending(x => x.InizioDeposito).ToList();
					else if (sede == 0)
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).ToList();
					else if (stagione == 0)
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderByDescending(x => x.InizioDeposito).ToList();
					else
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).ToList();
				}
				else if (inizioOrderByDesc == false)
				{
					if (sede == 0 && stagione == 0)
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false).OrderBy(x => x.InizioDeposito).ToList();
					else if (sede == 0)
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).ToList();
					else if (stagione == 0)
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderBy(x => x.InizioDeposito).ToList();
					else
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).ToList();
				}
			}
			else if (fineOrderByDesc != null)
			{
				if (fineOrderByDesc == true)
				{
					if (sede == 0 && stagione == 0)
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false).OrderByDescending(x => x.FineDeposito).ToList();
					else if (sede == 0)
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.FineDeposito).ToList();
					else if (stagione == 0)
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderByDescending(x => x.FineDeposito).ToList();
					else
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.FineDeposito).ToList();
				}
				else if (fineOrderByDesc == false)
				{
					if (sede == 0 && stagione == 0)
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false).OrderBy(x => x.FineDeposito).ToList();
					else if (sede == 0)
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.FineDeposito).ToList();
					else if (stagione == 0)
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderBy(x => x.FineDeposito).ToList();
					else
						result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.FineDeposito).ToList();
				}
			}
			return new Response<List<InventarioDTO>>(result, result.Count, HttpStatusCode.OK, null);
		}
		public async Task<Response<List<InventarioDTO>>> GetArchivio(int skip, int take, int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc, string targa = null)
		{
			string json = File.ReadAllText("inventario.json");
			var collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			var result = new List<InventarioDTO>();
			int count = 0;
			if (targa == null)
			{
				if (inizioOrderByDesc != null)
				{
					if (inizioOrderByDesc == true)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
					else if (inizioOrderByDesc == false)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
				}
				else if (fineOrderByDesc != null)
				{
					if (fineOrderByDesc == true)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
					else if (fineOrderByDesc == false)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
				}
			}
			else
			{
				if (inizioOrderByDesc != null)
				{
					if (inizioOrderByDesc == true)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
					else if (inizioOrderByDesc == false)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
				}
				else if (fineOrderByDesc != null)
				{
					if (fineOrderByDesc == true)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
					else if (fineOrderByDesc == false)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.FineDeposito != null && x.IsDeleted == false && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
				}
			}
			return new Response<List<InventarioDTO>>(result, count, HttpStatusCode.OK, null);
		}
		public async Task<Response<bool>> DelFromArchivio(InventarioDTO item)
		{
			if (await IsAlreadyExists(item) == false)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, "Non trovato");
			string json = File.ReadAllText("inventario.json");
			var collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			collection.Find(x => x.PneumaticiId == item.PneumaticiId && x.InizioDeposito == item.InizioDeposito).IsDeleted = true;
			File.WriteAllText("inventario.json", JsonConvert.SerializeObject(collection));
			return new Response<bool>(true, 1, HttpStatusCode.OK, "Spostato nel cestino");
		}
		public async Task<Response<List<InventarioDTO>>> GetAllCestino(int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc)
		{
			string json = File.ReadAllText("inventario.json");
			var collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			var result = new List<InventarioDTO>();
			if (inizioOrderByDesc != null)
			{
				if (inizioOrderByDesc == true)
				{
					if (sede == 0 && stagione == 0)
						result = collection.Where(x => x.IsDeleted == true).OrderByDescending(x => x.InizioDeposito).ToList();
					else if (sede == 0)
						result = collection.Where(x => x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).ToList();
					else if (stagione == 0)
						result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede).OrderByDescending(x => x.InizioDeposito).ToList();
					else
						result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).ToList();
				}
				else if (inizioOrderByDesc == false)
				{
					if (sede == 0 && stagione == 0)
						result = collection.Where(x => x.IsDeleted == true).OrderBy(x => x.InizioDeposito).ToList();
					else if (sede == 0)
						result = collection.Where(x => x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).ToList();
					else if (stagione == 0)
						result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede).OrderBy(x => x.InizioDeposito).ToList();
					else
						result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).ToList();
				}
			}
			else if (fineOrderByDesc != null)
			{
				if (inizioOrderByDesc == true)
				{
					if (sede == 0 && stagione == 0)
						result = collection.Where(x => x.IsDeleted == true).OrderByDescending(x => x.FineDeposito).ToList();
					else if (sede == 0)
						result = collection.Where(x => x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.FineDeposito).ToList();
					else if (stagione == 0)
						result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede).OrderByDescending(x => x.FineDeposito).ToList();
					else
						result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.FineDeposito).ToList();
				}
				else if (inizioOrderByDesc == false)
				{
					if (sede == 0 && stagione == 0)
						result = collection.Where(x => x.IsDeleted == true).OrderBy(x => x.FineDeposito).ToList();
					else if (sede == 0)
						result = collection.Where(x => x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.FineDeposito).ToList();
					else if (stagione == 0)
						result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede).OrderBy(x => x.FineDeposito).ToList();
					else
						result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.FineDeposito).ToList();
				}
			}
			return new Response<List<InventarioDTO>>(result, result.Count, HttpStatusCode.OK, null);
		}
		public async Task<Response<List<InventarioDTO>>> GetCestino(int skip, int take, int sede, int stagione, bool? inizioOrderByDesc, bool? fineOrderByDesc, string targa = null)
		{
			string json = File.ReadAllText("inventario.json");
			var collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			var result = new List<InventarioDTO>();
			int count = 0;
			if (targa == null)
			{
				if (inizioOrderByDesc != null)
				{
					if (inizioOrderByDesc == true)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.IsDeleted == true).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
					else if (inizioOrderByDesc == false)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.IsDeleted == true).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
				}
				else if (fineOrderByDesc != null)
				{
					if (fineOrderByDesc == true)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.IsDeleted == true).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
					else if (fineOrderByDesc == false)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.IsDeleted == true).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
				}
			}
			else
			{
				if (inizioOrderByDesc != null)
				{
					if (inizioOrderByDesc == true)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
					else if (inizioOrderByDesc == false)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.InizioDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
				}
				else if (fineOrderByDesc != null)
				{
					if (fineOrderByDesc == true)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderByDescending(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
					else if (fineOrderByDesc == false)
					{
						if (sede == 0 && stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true).Count();
						}
						else if (sede == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Pneumatici.StagioneId == stagione).Count();
						}
						else if (stagione == 0)
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede).Count();
						}
						else
						{
							result = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).OrderBy(x => x.FineDeposito).Skip(skip).Take(take).ToList();
							count = collection.Where(x => x.Pneumatici.Veicolo.Targa == targa && x.IsDeleted == true && x.Deposito.SedeId == sede && x.Pneumatici.StagioneId == stagione).Count();
						}
					}
				}
			}
			return new Response<List<InventarioDTO>>(result, count, HttpStatusCode.OK, null);
		}
		public async Task<Response<bool>> DelFromCestino(InventarioDTO item)
		{
			if (await IsAlreadyExists(item) == false)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, "Non trovato");
			string json = File.ReadAllText("inventario.json");
			var collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			collection.Remove(item);
			File.WriteAllText("inventario.json", JsonConvert.SerializeObject(collection));
			return new Response<bool>(true, 1, HttpStatusCode.OK, "Eliminato con successo");
		}
		public async Task<Response<bool>> RipristinaFromCestino(InventarioDTO item)
		{
			if (await IsAlreadyExists(item) == false)
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, "Non trovato");
			string json = File.ReadAllText("inventario.json");
			var collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			collection.Find(x => x.PneumaticiId == item.PneumaticiId && x.InizioDeposito == item.InizioDeposito).IsDeleted = false;
			File.WriteAllText("inventario.json", JsonConvert.SerializeObject(collection));
			return new Response<bool>(true, 1, HttpStatusCode.OK, "Spostato in archivio");
		}
		public async Task<Response<bool>> RipristinaCestino()
		{
			string json = File.ReadAllText("inventario.json");
			var collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			foreach (var i in collection)
				i.IsDeleted = false;
			File.WriteAllText("inventario.json", JsonConvert.SerializeObject(collection));
			return new Response<bool>(true, 1, HttpStatusCode.OK, "Cestino ripristinato");
		}
		public async Task<Response<bool>> SvuotaCestino()
		{
			string json = File.ReadAllText("inventario.json");
			var collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			List<InventarioDTO> toDelete = collection.Where(i => i.IsDeleted == true).ToList();
			foreach (var i in toDelete)
				collection.Remove(i);
			File.WriteAllText("inventario.json", JsonConvert.SerializeObject(collection));
			return new Response<bool>(true, 1, HttpStatusCode.OK, "Cestino svuotato");
		}
		#endregion

		#region UTILITIES
		private async Task<bool> IsAlreadyExists(InventarioDTO item)
		{
			string json = File.ReadAllText("inventario.json");
			var collection = JsonConvert.DeserializeObject<List<InventarioDTO>>(json);
			return collection.Any(x => x.PneumaticiId == item.PneumaticiId && x.InizioDeposito == item.InizioDeposito);
		}
		#endregion
	}
}
