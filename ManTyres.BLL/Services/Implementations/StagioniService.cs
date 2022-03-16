using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Implementations
{
	public class StagioniService : IStagioniService
	{
		public StagioniService() { }

		public async Task<Response<List<StagioniDTO>>> GetAllStagioni()
		{
			string json = File.ReadAllText("stagioni.json");
			List<StagioniDTO> stagioni = JsonConvert.DeserializeObject<List<StagioniDTO>>(json);
			if (stagioni.Count == 0)
				return new Response<List<StagioniDTO>>(null, 0, HttpStatusCode.OK, "Nessuna sede trovata");
			return new Response<List<StagioniDTO>>(stagioni, stagioni.Count, HttpStatusCode.OK, null);
		}

		public async Task<Response<bool>> AddStagioni()
		{
			return null;/*
            var _inverno = new Stagioni();
            _inverno.Nome = "inverno";
            var _estate = new Stagioni();
            _estate.Nome = "estate";
            await _context.Stagioni.AddRangeAsync(_inverno, _estate);
            if (await _context.SaveChangesAsync() < 1)
                return new Response<bool>(false, 0, HttpStatusCode.InternalServerError, "Impossibile salvare nel database");
            return new Response<bool>(true, 1, HttpStatusCode.Created, "Aggiunta con successo");*/
		}
	}
}
