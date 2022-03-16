using AutoMapper;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using ManTyres.DAL.SQLServer;
using ManTyres.DAL.SQLServer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Implementations
{
	public class DepositiService : IDepositiService
	{
		private readonly TyreDbContext _context;
		private readonly IMapper _mapper;

		public DepositiService(TyreDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<Response<List<DepositiDTO>>> GetAll()
		{
			var result = await _context.Depositi.Where(x => x.IsDeleted == false).OrderBy(x => x.Ubicazione).ToListAsync();
			foreach (var item in result)
				item.Sede = await _context.Sedi.FindAsync(item.SedeId);
			return Response(result);
		}

		public async Task<Response<bool>> Add(DepositiDTO data)
		{
			if (await IsAlreadyExists(data))
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Ubicazione già esistente!");
			data.IsDeleted = false;
			await _context.Depositi.AddAsync(_mapper.Map<Depositi>(data));
			return await SaveChanges("Aggiunto con successo");
		}

		public async Task<Response<bool>> Update(DepositiDTO data)
		{
			if (string.IsNullOrWhiteSpace(await CheckId(data.DepositoId)))
				return new Response<bool>(false, 0, HttpStatusCode.NotFound, await CheckId(data.DepositoId));
			if (await IsAlreadyExists(data))
				return new Response<bool>(false, 0, HttpStatusCode.UnprocessableEntity, "Ubicazione già esistente!");
			_context.Depositi.Update(_mapper.Map<Depositi>(data));
			return await SaveChanges("Modificato con successo");
		}


		#region UTILITIES
		private async Task<string> CheckId(int id)
		{
			if (await _context.Depositi.AnyAsync(x => x.DepositoId == id) == false)
				return $"Nessuno con Id = {id}";
			return null;
		}

		private async Task<bool> IsAlreadyExists(DepositiDTO deposito)
		{
			return await _context.Depositi.AnyAsync(x => x.DepositoId != x.DepositoId && x.Ubicazione == deposito.Ubicazione);
		}

		private Response<List<DepositiDTO>> Response(List<Depositi> data)
		{
			if (data.Count == 0)
				return new Response<List<DepositiDTO>>(null, 0, HttpStatusCode.OK, "Nessun deposito trovato");
			return new Response<List<DepositiDTO>>(data.ConvertAll(_mapper.Map<DepositiDTO>), data.Count, HttpStatusCode.OK, null);
		}

		private async Task<Response<bool>> SaveChanges(string successMessage, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			var result = await _context.SaveChangesAsync();
			if (result < 1)
				return new Response<bool>(false, result, HttpStatusCode.InternalServerError, "Impossibile salvare nel database");
			return new Response<bool>(true, result, statusCode, successMessage);
		}
		#endregion
	}
}
