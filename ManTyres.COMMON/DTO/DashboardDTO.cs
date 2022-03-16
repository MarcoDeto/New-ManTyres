using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManTyres.COMMON.DTO
{
	#nullable disable

	public class DashboardDTO
	{
		public int Count { get; set; }
		public int CountTOT { get; set; }
	}

	public class ChartDTO
	{
		public ChartDTO(string date, int value)
		{
			Date = date;
			Value = value;
		}
		public string Date { get; set; }
		public int Value { get; set; }
	}

	public class YearChartDTO
	{
		public YearChartDTO(int year, int value)
		{
			Year = year;
			Value = value;
		}
		public int Year { get; set; }
		public int Value { get; set; }
	}

	public class ReportDTO
	{
		public ReportDTO(DateTime date, int value)
		{
			Date = date;
			Value = value;
		}
		public DateTime Date { get; set; }
		public int Value { get; set; }
	}

}
