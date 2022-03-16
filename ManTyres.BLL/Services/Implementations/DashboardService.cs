using ManTyres.BLL.Services.Interfaces;
using ManTyres.COMMON.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ManTyres.BLL.Services.Implementations
{
	public class DashboardService : IDashboardService
	{
		public DashboardService() { }

		public Response<int> TotalePneumatici()
		{
			string json = File.ReadAllText("pneumatici.json");
			List<PneumaticiDTO> pneumatici = JsonConvert.DeserializeObject<List<PneumaticiDTO>>(json);
			var result = new DashboardDTO()
			{
				Count = pneumatici.Where(x => x.IsDeleted == false).Distinct().Count(),
				CountTOT = pneumatici.Count()
			};
			return new Response<int>(result.Count, result.CountTOT, HttpStatusCode.OK, null);
		}
		public Response<int> TotaleVeicoli()
		{
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			var result = new DashboardDTO()
			{
				Count = veicoli.Where(x => x.IsDeleted == false).Distinct().Count(),
				CountTOT = veicoli.Count()
			};
			return new Response<int>(result.Count, result.CountTOT, HttpStatusCode.OK, null);
		}
		public Response<int> TotaleClienti()
		{
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			var result = new DashboardDTO()
			{
				Count = clienti.Where(x => x.IsDeleted == false).Distinct().Count(),
				CountTOT = clienti.Count()
			};
			return new Response<int>(result.Count, result.CountTOT, HttpStatusCode.OK, null);
		}
		public Response<int> TotaleUtenti()
		{
			string json = File.ReadAllText("users.json");
			List<UserDTO> users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
			var result = new DashboardDTO()
			{
				Count = users.Where(x => x.IsDeleted == false).Distinct().Count(),
				CountTOT = users.Count()
			};
			return new Response<int>(result.Count, result.CountTOT, HttpStatusCode.OK, null);
		}

		public async Task<Response<List<ChartDTO>>> ChartQuantitàPneumatici(int month, string ci)
		{
			string errorMessage = "pneumatico";
			string json = File.ReadAllText("pneumatici.json");
			List<PneumaticiDTO> pneumatici = JsonConvert.DeserializeObject<List<PneumaticiDTO>>(json);
			if (pneumatici.Any() == false)
				return new Response<List<ChartDTO>>(null, 0, HttpStatusCode.OK, $"Nussun {errorMessage} trovato");
			CultureInfo cultureInfo = new CultureInfo(ci);
			var result = new List<ChartDTO>();
			switch (month)
			{
				case 0:
					return Response(await ChartPneumatici7days(result, cultureInfo), errorMessage);

				case 1:
					return Response(await ChartPneumatici30days(result, cultureInfo), errorMessage);

				case 1000:
					return Response(await ChartPneumaticiAlways(result), errorMessage);

				default:
					return Response(await ChartPneumaticiMonths(result, cultureInfo, month), errorMessage);
			}
		}
		public async Task<Response<List<ChartDTO>>> ChartGlobalePneumatici(int month, string ci)
		{
			string json = File.ReadAllText("pneumatici.json");
			List<PneumaticiDTO> pneumatici = JsonConvert.DeserializeObject<List<PneumaticiDTO>>(json);
			string errorMessage = "pneumatico";
			if (pneumatici.Any() == false)
				return new Response<List<ChartDTO>>(null, 0, HttpStatusCode.OK, $"Nussun {errorMessage} trovato");
			CultureInfo cultureInfo = new CultureInfo(ci);
			var result = new List<ChartDTO>();
			var start = DateTime.Now;
			int value = 0;
			switch (month)
			{
				case 0:
					start = DateTime.Today.AddDays(-7 + 1);
					result = await ChartPneumatici7days(result, cultureInfo);
					value = pneumatici
						 .Where(x => x.DataUbicazione < start && x.IsDeleted == false)
						 .Count();
					return SumChartGlobale(value, result, errorMessage);

				case 1:
					start = DateTime.Today.AddDays(-30 + 1);
					result = await ChartPneumatici30days(result, cultureInfo);
					value = pneumatici
						 .Where(x => x.DataUbicazione < start && x.IsDeleted == false)
						 .Count();
					return SumChartGlobale(value, result, errorMessage);

				case 1000:
					start = pneumatici.Where(x => x.IsDeleted == false).OrderBy(x => x.DataUbicazione).First().DataUbicazione;
					result = await ChartPneumaticiAlways(result);
					result = SumChartGlobale(0, result, errorMessage).Content;
					value = pneumatici
						 .Where(x => x.DataUbicazione < start.AddDays(1) && x.IsDeleted == false)
						 .Count();
					result.Insert(0, new ChartDTO(start.ToString("d", cultureInfo), value));
					return new Response<List<ChartDTO>>(result, result.Count, HttpStatusCode.OK, null);

				default:
					start = DateTime.Today.AddMonths(-month).AddDays(DateTime.Today.Day);
					result = await ChartPneumaticiMonths(result, cultureInfo, month);
					value = pneumatici
						 .Where(x => x.DataUbicazione < start && x.IsDeleted == false)
						 .Count();
					return SumChartGlobale(value, result, errorMessage);
			}
		}
		public async Task<Response<List<ChartDTO>>> ChartQuantitàVeicoli(int month, string ci)
		{
			string errorMessage = "veicolo";
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			if (veicoli.Any() == false)
				return new Response<List<ChartDTO>>(null, 0, HttpStatusCode.OK, $"Nussun {errorMessage} trovato");
			CultureInfo cultureInfo = new CultureInfo(ci);
			var result = new List<ChartDTO>();
			switch (month)
			{
				case 0:
					return Response(await ChartVeicoli7days(result, cultureInfo), errorMessage);

				case 1:
					return Response(await ChartVeicoli30days(result, cultureInfo), errorMessage);

				case 1000:
					return Response(await ChartVeicoliAlways(result), errorMessage);

				default:
					return Response(await ChartVeicoliMonths(result, cultureInfo, month), errorMessage);
			}
		}
		public async Task<Response<List<ChartDTO>>> ChartGlobaleVeicoli(int month, string ci)
		{
			string errorMessage = "veicolo";
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			if (veicoli.Any() == false)
				return new Response<List<ChartDTO>>(null, 0, HttpStatusCode.OK, $"Nussun {errorMessage} trovato");
			CultureInfo cultureInfo = new CultureInfo(ci);
			var result = new List<ChartDTO>();
			var start = DateTime.Now;
			int value = 0;
			switch (month)
			{
				case 0:
					start = DateTime.Today.AddDays(-7 + 1);
					result = await ChartVeicoli7days(result, cultureInfo);
					value = veicoli
						 .Where(x => x.DataCreazione < start && x.IsDeleted == false)
						 .Count();
					return SumChartGlobale(value, result, errorMessage);

				case 1:
					start = DateTime.Today.AddDays(-30 + 1);
					result = await ChartVeicoli30days(result, cultureInfo);
					value = veicoli
						 .Where(x => x.DataCreazione < start && x.IsDeleted == false)
						 .Count();
					return SumChartGlobale(value, result, errorMessage);

				case 1000:
					start = veicoli.Where(x => x.IsDeleted == false).OrderBy(x => x.DataCreazione).First().DataCreazione;
					result = await ChartVeicoliAlways(result);
					result = SumChartGlobale(0, result, errorMessage).Content;
					value = veicoli
						 .Where(x => x.DataCreazione < start.AddDays(1) && x.IsDeleted == false)
						 .Count();
					result.Insert(0, new ChartDTO(start.ToString("d", cultureInfo), value));
					return new Response<List<ChartDTO>>(result, result.Count, HttpStatusCode.OK, null);

				default:
					start = DateTime.Today.AddMonths(-month).AddDays(DateTime.Today.Day);
					result = await ChartVeicoliMonths(result, cultureInfo, month);
					value = veicoli
						 .Where(x => x.DataCreazione < start && x.IsDeleted == false)
						 .Count();
					return SumChartGlobale(value, result, errorMessage);
			}
		}
		public async Task<Response<List<ChartDTO>>> ChartQuantitàClienti(int month, string ci)
		{
			string errorMessage = "cliente";
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			if (clienti.Any() == false)
				return new Response<List<ChartDTO>>(null, 0, HttpStatusCode.OK, $"Nussun {errorMessage} trovato");
			CultureInfo cultureInfo = new CultureInfo(ci);
			var result = new List<ChartDTO>();
			switch (month)
			{
				case 0:
					return Response(await ChartClienti7days(result, cultureInfo), errorMessage);

				case 1:
					return Response(await ChartClienti30days(result, cultureInfo), errorMessage);

				case 1000:
					return Response(await ChartClientiAlways(result), errorMessage);

				default:
					return Response(await ChartClientiMonths(result, cultureInfo, month), errorMessage);
			}
		}
		public async Task<Response<List<ChartDTO>>> ChartGlobaleClienti(int month, string ci)
		{
			string errorMessage = "cliente";
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			if (clienti.Any() == false)
				return new Response<List<ChartDTO>>(null, 0, HttpStatusCode.OK, $"Nussun {errorMessage} trovato");
			CultureInfo cultureInfo = new CultureInfo(ci);
			var result = new List<ChartDTO>();
			var start = DateTime.Now;
			int value = 0;
			switch (month)
			{
				case 0:
					start = DateTime.Today.AddDays(-7 + 1);
					result = await ChartClienti7days(result, cultureInfo);
					value = clienti
						 .Where(x => x.DataCreazione < start && x.IsDeleted == false)
						 .Count();
					return SumChartGlobale(value, result, errorMessage);

				case 1:
					start = DateTime.Today.AddDays(-30 + 1);
					result = await ChartClienti30days(result, cultureInfo);
					value = clienti
						 .Where(x => x.DataCreazione < start && x.IsDeleted == false)
						 .Count();
					return SumChartGlobale(value, result, errorMessage);

				case 1000:
					start = clienti.Where(x => x.IsDeleted == false).OrderBy(x => x.DataCreazione).First().DataCreazione;
					result = await ChartClientiAlways(result);
					result = SumChartGlobale(0, result, errorMessage).Content;
					value = clienti
						 .Where(x => x.DataCreazione < start.AddDays(1) && x.IsDeleted == false)
						 .Count();
					result.Insert(0, new ChartDTO(start.ToString("d", cultureInfo), value));
					return new Response<List<ChartDTO>>(result, result.Count, HttpStatusCode.OK, null);

				default:
					start = DateTime.Today.AddMonths(-month).AddDays(DateTime.Today.Day);
					result = await ChartClientiMonths(result, cultureInfo, month);
					value = clienti
						 .Where(x => x.DataCreazione < start && x.IsDeleted == false)
						 .Count();
					return SumChartGlobale(value, result, errorMessage);
			}
		}


		#region UTILITIES
		public async Task<List<ChartDTO>> ChartPneumatici7days(List<ChartDTO> result, CultureInfo cultureInfo)
		{
			DateTime start = DateTime.Today.AddDays(-7 + 1);

			var last7Days = Enumerable.Range(0, 7).Select(i => DateTime.Now.Date.AddDays(i - 7 + 1));
			string json = File.ReadAllText("pneumatici.json");
			List<PneumaticiDTO> pneumatici = JsonConvert.DeserializeObject<List<PneumaticiDTO>>(json);
			var groups7Day = pneumatici
				 .Where(x => x.DataUbicazione >= start && x.DataUbicazione <= DateTime.Now.AddDays(1) && x.IsDeleted == false)
				 .GroupBy(x => x.DataUbicazione.Date)
				 .Select(x => new ReportDTO(x.Key, x.Count()))
				 .ToList();

			result.AddRange(last7Days.Select(item => new ChartDTO(
				 item.ToString("m", cultureInfo),
				 groups7Day.Any(c => item == c.Date)
				 ? groups7Day.FirstOrDefault(c => item.Date == c.Date).Value
				 : 0
				 )
			));
			return result;
		}
		public async Task<List<ChartDTO>> ChartPneumatici30days(List<ChartDTO> result, CultureInfo cultureInfo)
		{
			DateTime start = DateTime.Today.AddDays(-30 + 1);

			var last30Days = Enumerable.Range(0, 30).Select(i => DateTime.Now.Date.AddDays(i - 30 + 1));
			string json = File.ReadAllText("pneumatici.json");
			List<PneumaticiDTO> pneumatici = JsonConvert.DeserializeObject<List<PneumaticiDTO>>(json);
			var groups30Day = pneumatici
				 .Where(x => x.DataUbicazione >= start && x.DataUbicazione <= DateTime.Now.AddDays(1) && x.IsDeleted == false)
				 .GroupBy(x => x.DataUbicazione.Date)
				 .Select(x => new ReportDTO(x.Key, x.Count()))
				 .ToList();

			result.AddRange(last30Days.Select(item => new ChartDTO(
				 item.ToString("m", cultureInfo),
				 groups30Day.Any(c => item == c.Date)
				 ? groups30Day.FirstOrDefault(c => item == c.Date).Value
				 : 0
				 )
			));
			return result;
		}
		public async Task<List<ChartDTO>> ChartPneumaticiMonths(List<ChartDTO> result, CultureInfo cultureInfo, int month)
		{
			DateTime start = DateTime.Today.AddMonths(-month).AddDays(DateTime.Today.Day);

			var lastMonths = Enumerable.Range(0, month)
				 .Select(i => DateTime.Now.AddMonths(i - month + 1))
				 .Select(date => date.Month);
			string json = File.ReadAllText("pneumatici.json");
			List<PneumaticiDTO> pneumatici = JsonConvert.DeserializeObject<List<PneumaticiDTO>>(json);
			var groupMonth = pneumatici
				 .Where(x => x.DataUbicazione >= start && x.DataUbicazione <= DateTime.Now.AddDays(1) && x.IsDeleted == false)
				 .GroupBy(x => x.DataUbicazione.Month)
				 .Select(x => new YearChartDTO(x.Key, x.Count()))
				 .ToList();

			result.AddRange(lastMonths.Select(item => new ChartDTO(
				 new DateTime(2021, item, 1).ToString("MMMM", cultureInfo),
				 groupMonth.Any(d => item == d.Year)
				 ? groupMonth.FirstOrDefault(d => item == d.Year).Value
				 : 0
				 )
			));
			return result;
		}
		public async Task<List<ChartDTO>> ChartPneumaticiAlways(List<ChartDTO> result)
		{
			string json = File.ReadAllText("pneumatici.json");
			List<PneumaticiDTO> pneumatici = JsonConvert.DeserializeObject<List<PneumaticiDTO>>(json);
			DateTime start = pneumatici.Where(x => x.IsDeleted == false).OrderBy(x => x.DataUbicazione).First().DataUbicazione;

			var lastYears = Enumerable.Range(0, DateTime.Now.Year - start.Year + 1)
				 .Select(i => DateTime.Now.AddYears(i - (DateTime.Now.Year - start.Year)))
				 .Select(date => date.Year);

			var groupYear = pneumatici
				 .Where(x => x.DataUbicazione >= start && x.DataUbicazione <= DateTime.Now.AddDays(1) && x.IsDeleted == false)
				 .GroupBy(x => x.DataUbicazione.Year)
				 .Select(x => new YearChartDTO(x.Key, x.Count()))
				 .ToList();

			result.AddRange(lastYears.Select(item => new ChartDTO(
				 item.ToString(),
				 groupYear.Any(d => item == d.Year)
				 ? groupYear.FirstOrDefault(d => item == d.Year).Value
				 : 0
				 )
			));
			return result;
		}

		public async Task<List<ChartDTO>> ChartVeicoli7days(List<ChartDTO> result, CultureInfo cultureInfo)
		{
			DateTime start = DateTime.Today.AddDays(-7 + 1);

			var last7Days = Enumerable.Range(0, 7).Select(i => DateTime.Now.Date.AddDays(i - 7 + 1));
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			var groups7Day = veicoli
				 .Where(x => x.DataCreazione >= start && x.DataCreazione <= DateTime.Now.AddDays(1) && x.IsDeleted == false)
				 .GroupBy(x => x.DataCreazione.Date)
				 .Select(x => new ReportDTO(x.Key, x.Count()))
				 .ToList();

			result.AddRange(last7Days.Select(item => new ChartDTO(
				 item.ToString("m", cultureInfo),
				 groups7Day.Any(c => item == c.Date)
				 ? groups7Day.FirstOrDefault(c => item.Date == c.Date).Value
				 : 0
				 )
			));
			return result;
		}
		public async Task<List<ChartDTO>> ChartVeicoli30days(List<ChartDTO> result, CultureInfo cultureInfo)
		{
			DateTime start = DateTime.Today.AddDays(-30 + 1);

			var last30Days = Enumerable.Range(0, 30).Select(i => DateTime.Now.Date.AddDays(i - 30 + 1));
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			var groups30Day = veicoli
				 .Where(x => x.DataCreazione >= start && x.DataCreazione <= DateTime.Now.AddDays(1) && x.IsDeleted == false)
				 .GroupBy(x => x.DataCreazione.Date)
				 .Select(x => new ReportDTO(x.Key, x.Count()))
				 .ToList();

			result.AddRange(last30Days.Select(item => new ChartDTO(
				 item.ToString("m", cultureInfo),
				 groups30Day.Any(c => item == c.Date)
				 ? groups30Day.FirstOrDefault(c => item == c.Date).Value
				 : 0
				 )
			));
			return result;
		}
		public async Task<List<ChartDTO>> ChartVeicoliMonths(List<ChartDTO> result, CultureInfo cultureInfo, int month)
		{
			DateTime start = DateTime.Today.AddMonths(-month).AddDays(DateTime.Today.Day);

			var lastMonths = Enumerable.Range(0, month)
				 .Select(i => DateTime.Now.AddMonths(i - month + 1))
				 .Select(date => date.Month);
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			var groupMonth = veicoli
				 .Where(x => x.DataCreazione >= start && x.DataCreazione <= DateTime.Now.AddDays(1) && x.IsDeleted == false)
				 .GroupBy(x => x.DataCreazione.Month)
				 .Select(x => new YearChartDTO(x.Key, x.Count()))
				 .ToList();

			result.AddRange(lastMonths.Select(item => new ChartDTO(
				 new DateTime(2021, item, 1).ToString("MMMM", cultureInfo),
				 groupMonth.Any(d => item == d.Year)
				 ? groupMonth.FirstOrDefault(d => item == d.Year).Value
				 : 0
				 )
			));
			return result;
		}
		public async Task<List<ChartDTO>> ChartVeicoliAlways(List<ChartDTO> result)
		{
			string json = File.ReadAllText("veicoli.json");
			List<VeicoliDTO> veicoli = JsonConvert.DeserializeObject<List<VeicoliDTO>>(json);
			DateTime start = veicoli.Where(x => x.IsDeleted == false).OrderBy(x => x.DataCreazione).First().DataCreazione;

			var lastYears = Enumerable.Range(0, DateTime.Now.Year - start.Year + 1)
				 .Select(i => DateTime.Now.AddYears(i - (DateTime.Now.Year - start.Year)))
				 .Select(date => date.Year);

			var groupYear = veicoli
				 .Where(x => x.DataCreazione >= start && x.DataCreazione <= DateTime.Now.AddDays(1) && x.IsDeleted == false)
				 .GroupBy(x => x.DataCreazione.Year)
				 .Select(x => new YearChartDTO(x.Key, x.Count()))
				 .ToList();

			result.AddRange(lastYears.Select(item => new ChartDTO(
				 item.ToString(),
				 groupYear.Any(d => item == d.Year)
				 ? groupYear.FirstOrDefault(d => item == d.Year).Value
				 : 0
				 )
			));
			return result;
		}

		public async Task<List<ChartDTO>> ChartClienti7days(List<ChartDTO> result, CultureInfo cultureInfo)
		{
			DateTime start = DateTime.Today.AddDays(-7 + 1);

			var last7Days = Enumerable.Range(0, 7).Select(i => DateTime.Now.Date.AddDays(i - 7 + 1));
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			var groups7Day = clienti
				 .Where(x => x.DataCreazione >= start && x.DataCreazione <= DateTime.Now.AddDays(1) && x.IsDeleted == false)
				 .GroupBy(x => x.DataCreazione.Date)
				 .Select(x => new ReportDTO(x.Key, x.Count()))
				 .ToList();

			result.AddRange(last7Days.Select(item => new ChartDTO(
				 item.ToString("m", cultureInfo),
				 groups7Day.Any(c => item == c.Date)
				 ? groups7Day.FirstOrDefault(c => item.Date == c.Date).Value
				 : 0
				 )
			));
			return result;
		}
		public async Task<List<ChartDTO>> ChartClienti30days(List<ChartDTO> result, CultureInfo cultureInfo)
		{
			DateTime start = DateTime.Today.AddDays(-30 + 1);

			var last30Days = Enumerable.Range(0, 30).Select(i => DateTime.Now.Date.AddDays(i - 30 + 1));
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			var groups30Day = clienti
				 .Where(x => x.DataCreazione >= start && x.DataCreazione <= DateTime.Now.AddDays(1) && x.IsDeleted == false)
				 .GroupBy(x => x.DataCreazione.Date)
				 .Select(x => new ReportDTO(x.Key, x.Count()))
				 .ToList();

			result.AddRange(last30Days.Select(item => new ChartDTO(
				 item.ToString("m", cultureInfo),
				 groups30Day.Any(c => item == c.Date)
				 ? groups30Day.FirstOrDefault(c => item == c.Date).Value
				 : 0
				 )
			));
			return result;
		}
		public async Task<List<ChartDTO>> ChartClientiMonths(List<ChartDTO> result, CultureInfo cultureInfo, int month)
		{
			DateTime start = DateTime.Today.AddMonths(-month).AddDays(DateTime.Today.Day);

			var lastMonths = Enumerable.Range(0, month)
				 .Select(i => DateTime.Now.AddMonths(i - month + 1))
				 .Select(date => date.Month);
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			var groupMonth = clienti
				 .Where(x => x.DataCreazione >= start && x.DataCreazione <= DateTime.Now.AddDays(1) && x.IsDeleted == false)
				 .GroupBy(x => x.DataCreazione.Month)
				 .Select(x => new YearChartDTO(x.Key, x.Count()))
				 .ToList();

			result.AddRange(lastMonths.Select(item => new ChartDTO(
				 new DateTime(2021, item, 1).ToString("MMMM", cultureInfo),
				 groupMonth.Any(d => item == d.Year)
				 ? groupMonth.FirstOrDefault(d => item == d.Year).Value
				 : 0
				 )
			));
			return result;
		}
		public async Task<List<ChartDTO>> ChartClientiAlways(List<ChartDTO> result)
		{
			string json = File.ReadAllText("clienti.json");
			List<ClientiDTO> clienti = JsonConvert.DeserializeObject<List<ClientiDTO>>(json);
			DateTime start = clienti.Where(x => x.IsDeleted == false).OrderBy(x => x.DataCreazione).First().DataCreazione;

			var lastYears = Enumerable.Range(0, DateTime.Now.Year - start.Year + 1)
				 .Select(i => DateTime.Now.AddYears(i - (DateTime.Now.Year - start.Year)))
				 .Select(date => date.Year);

			var groupYear = clienti
				 .Where(x => x.DataCreazione >= start && x.DataCreazione <= DateTime.Now.AddDays(1) && x.IsDeleted == false)
				 .GroupBy(x => x.DataCreazione.Year)
				 .Select(x => new YearChartDTO(x.Key, x.Count()))
				 .ToList();

			result.AddRange(lastYears.Select(item => new ChartDTO(
				 item.ToString(),
				 groupYear.Any(d => item == d.Year)
				 ? groupYear.FirstOrDefault(d => item == d.Year).Value
				 : 0
				 )
			));
			return result;
		}

		public Response<List<ChartDTO>> SumChartGlobale(int value, List<ChartDTO> result, string errorMessage)
		{
			if (result == null)
				return new Response<List<ChartDTO>>(null, 0, HttpStatusCode.OK, $"Nussun {errorMessage} trovato");
			else if (result.Count == 0)
				return new Response<List<ChartDTO>>(null, 0, HttpStatusCode.OK, $"Nessun {errorMessage} salvato nel database");
			foreach (var item in result)
			{
				value += item.Value;
				item.Value = value;
			}
			return new Response<List<ChartDTO>>(result, result.Count, HttpStatusCode.OK, null);
		}
		public Response<List<ChartDTO>> Response(List<ChartDTO> result, string errorMessage)
		{
			if (result == null)
				return new Response<List<ChartDTO>>(null, 0, HttpStatusCode.OK, $"Nussun {errorMessage} trovato");
			else if (result.Count == 0)
				return new Response<List<ChartDTO>>(null, 0, HttpStatusCode.OK, $"Nessun {errorMessage} salvato nel database");
			return new Response<List<ChartDTO>>(result, result.Count, HttpStatusCode.OK, null);
		}
		#endregion
	}
}
