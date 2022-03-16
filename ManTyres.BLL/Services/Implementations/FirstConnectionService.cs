using AutoMapper;
using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using ManTyres.DAL.SQLServer;
using ManTyres.DAL.SQLServer.Entities;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Implementations
{
	public class FirstConnectionService : IFirstConnectionService
	{
		private readonly TyreDbContext _context;
		private readonly IMapper _mapper;
		private readonly IRoleService _roleService;
		private readonly IStagioniService _stagioniService;
		public FirstConnectionService(TyreDbContext context, IMapper mapper, IRoleService roleService, IStagioniService stagioniService)
		{
			_context = context;
			_mapper = mapper;
			_roleService = roleService;
			_stagioniService = stagioniService;
		}

		public async Task<Response<SetupDTO>> GetSetup()
		{
			var result = new SetupDTO()
			{
				Ruoli = _context.Roles.Count(),
				Stagioni = _context.Stagioni.Count(),
				Utenti = _context.Users.Count(),
				Sedi = _context.Sedi.Count()
			};
			if (result.Ruoli == 0)
			{
				await _roleService.Create("admin");
				await _roleService.Create("user");
			}
			if (result.Stagioni == 0)
			{
				await _stagioniService.AddStagioni();
			}
			return new Response<SetupDTO>(result, 1, HttpStatusCode.OK, null);
		}

		public async Task<Response<bool>> AddFirstSede(SediDTO sede)
		{
			if (_context.Sedi.Count() > 0)
				return new Response<bool>(false, 0, HttpStatusCode.InternalServerError, "Esiste almeno una sede");
			sede.IsDeleted = false;
			await _context.Sedi.AddAsync(_mapper.Map<Sedi>(sede));
			if (await _context.SaveChangesAsync() != 1)
				return new Response<bool>(false, 0, HttpStatusCode.InternalServerError, $"Errore! Impossibile salvare nel database");
			return new Response<bool>(true, 1, HttpStatusCode.OK, "Prima sede inserita");
		}
	}
}
