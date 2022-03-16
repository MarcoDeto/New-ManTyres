using ClosedXML.Excel;
using ManTyres.BLL.Repository.Interfaces;
using ManTyres.COMMON.DTO;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace ManTyres.BLL.Repository.Implementations
{
	public class ExcelRepository : IExcelRepository
	{
		public ExcelRepository() { }

		public DataTable ImportExcel(Stream file)
		{
			// Open the Excel file using ClosedXML.
			// Keep in mind the Excel file cannot be open when trying to read it
			using (XLWorkbook workBook = new XLWorkbook(file))
			{
				//Read the first Sheet from Excel file.
				IXLWorksheet workSheet = workBook.Worksheet(1);

				//Create a new DataTable.
				DataTable dt = new DataTable();

				//Loop through the Worksheet rows.
				bool firstRow = true;
				int test = 0;
				foreach (IXLRow row in workSheet.Rows())
				{
					test++;
					//Use the first row to add columns to DataTable.
					if (firstRow)
					{
						foreach (IXLCell cell in row.Cells())
						{
							dt.Columns.Add(cell.Value.ToString().Trim());
						}
						firstRow = false;
					}
					else
					{
						//Add rows to DataTable.
						dt.Rows.Add();
						int i = 0;
						foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
						{
							dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
							i++;
						}
					}
				}
				return dt;
			}
		}

		public byte[] ExportClienti(List<ClientiDTO> clienti)
		{
			using (var workbook = new XLWorkbook())
			{
				IXLWorksheet worksheet = workbook.Worksheets.Add("Clienti");
				worksheet.Cell(1, 1).Value = "AZIENDA";
				worksheet.Cell(1, 2).Value = "NOME";
				worksheet.Cell(1, 3).Value = "CODICE FISCALE";
				worksheet.Cell(1, 4).Value = "PARTITA IVA";
				worksheet.Cell(1, 5).Value = "EMAIL";
				worksheet.Cell(1, 6).Value = "TELEFONO";
				worksheet.Cell(1, 7).Value = "INDIRIZZO";
				worksheet.Cell(1, 8).Value = "COMUNE";
				worksheet.Cell(1, 9).Value = "CAP";
				worksheet.Cell(1, 10).Value = "PROVINCIA";
				worksheet.Cell(1, 11).Value = "NAZIONE";
				worksheet.Cell(1, 12).Value = "DATA CREAZIONE";
				if (clienti != null)
				{
					for (int index = 1; index <= clienti.Count; index++)
					{
						worksheet.Cell(index + 1, 1).Value = clienti[index - 1].IsAzienda == true ? "X" : "";
						worksheet.Cell(index + 1, 2).Value = clienti[index - 1].Nome;
						worksheet.Cell(index + 1, 3).Value = clienti[index - 1].CodiceFiscale;
						worksheet.Cell(index + 1, 4).Value = clienti[index - 1].PartitaIva;
						worksheet.Cell(index + 1, 5).Value = clienti[index - 1].Email;
						worksheet.Cell(index + 1, 6).Value = clienti[index - 1].Telefono;
						worksheet.Cell(index + 1, 7).Value = clienti[index - 1].Indirizzo;
						worksheet.Cell(index + 1, 8).Value = clienti[index - 1].Comune;
						worksheet.Cell(index + 1, 9).Value = clienti[index - 1].Cap;
						worksheet.Cell(index + 1, 10).Value = clienti[index - 1].Provincia;
						worksheet.Cell(index + 1, 11).Value = clienti[index - 1].Nazione;
						worksheet.Cell(index + 1, 12).Value = clienti[index - 1].DataCreazione;
					}
				}
				using (var stream = new MemoryStream())
				{
					workbook.SaveAs(stream);
					var content = stream.ToArray();
					return content;
				}
			}
		}

		public byte[] ExportVeicoli(List<VeicoliDTO> veicoli)
		{
			using (var workbook = new XLWorkbook())
			{
				IXLWorksheet worksheet = workbook.Worksheets.Add("Veicoli");
				worksheet.Cell(1, 1).Value = "MARCA";
				worksheet.Cell(1, 2).Value = "MODELLO";
				worksheet.Cell(1, 3).Value = "TARGA";
				worksheet.Cell(1, 4).Value = "CLIENTE";
				worksheet.Cell(1, 5).Value = "DATA CREAZIONE";
				if (veicoli != null)
				{
					for (int index = 1; index <= veicoli.Count; index++)
					{
						worksheet.Cell(index + 1, 1).Value = veicoli[index - 1].Marca;
						worksheet.Cell(index + 1, 2).Value = veicoli[index - 1].Modello;
						worksheet.Cell(index + 1, 3).Value = veicoli[index - 1].Targa;
						if (veicoli[index - 1].Cliente != null)
						{
							worksheet.Cell(index + 1, 4).Value = veicoli[index - 1].Cliente.Nome + " " + veicoli[index - 1].Cliente.Cognome;
						}
						worksheet.Cell(index + 1, 5).Value = veicoli[index - 1].DataCreazione;
					}
				}
				using (var stream = new MemoryStream())
				{
					workbook.SaveAs(stream);
					var content = stream.ToArray();
					return content;
				}
			}
		}

		public byte[] ExportPneumatici(List<InventarioDTO> pneumatici)
		{
			using (var workbook = new XLWorkbook())
			{
				IXLWorksheet worksheet = workbook.Worksheets.Add("Pneumatici");
				worksheet.Cell(1, 1).Value = "STAGIONE";
				worksheet.Cell(1, 2).Value = "MARCA";
				worksheet.Cell(1, 3).Value = "MODELLO";
				worksheet.Cell(1, 4).Value = "MISURA";
				worksheet.Cell(1, 5).Value = "BATTISTRADA";
				worksheet.Cell(1, 6).Value = "DOT";
				worksheet.Cell(1, 7).Value = "STATO GOMME";
				worksheet.Cell(1, 8).Value = "Q.TA'";
				worksheet.Cell(1, 9).Value = "UBICAZIONE";
				worksheet.Cell(1, 10).Value = "SEDE";
				worksheet.Cell(1, 11).Value = "INIZIO DEPOSITO";
				worksheet.Cell(1, 12).Value = "FINE DEPOSITO";
				worksheet.Cell(1, 13).Value = "OPERATORE";
				worksheet.Cell(1, 14).Value = "TARGA";
				worksheet.Cell(1, 15).Value = "CLIENTE";
				worksheet.Cell(1, 16).Value = "AZIENDA";
				if (pneumatici != null)
				{
					for (int index = 1; index <= pneumatici.Count; index++)
					{
						worksheet.Cell(index + 1, 1).Value = pneumatici[index - 1].Pneumatici.Stagione.Nome;
						worksheet.Cell(index + 1, 2).Value = pneumatici[index - 1].Pneumatici.Marca;
						worksheet.Cell(index + 1, 3).Value = pneumatici[index - 1].Pneumatici.Modello;
						worksheet.Cell(index + 1, 4).Value = pneumatici[index - 1].Pneumatici.Misura;
						worksheet.Cell(index + 1, 5).Value = pneumatici[index - 1].Battistrada + " mm";
						worksheet.Cell(index + 1, 6).Value = pneumatici[index - 1].Pneumatici.Dot;
						worksheet.Cell(index + 1, 7).Value = pneumatici[index - 1].StatoGomme;
						worksheet.Cell(index + 1, 8).Value = pneumatici[index - 1].Pneumatici.Quantita;
						worksheet.Cell(index + 1, 9).Value = pneumatici[index - 1].Deposito.Ubicazione;
						worksheet.Cell(index + 1, 10).Value = pneumatici[index - 1].Deposito.Sede.Comune;
						worksheet.Cell(index + 1, 11).Value = pneumatici[index - 1].InizioDeposito;
						worksheet.Cell(index + 1, 12).Value = pneumatici[index - 1].FineDeposito;
						if (pneumatici[index - 1].User != null)
							worksheet.Cell(index + 1, 13).Value = pneumatici[index - 1].User.UserName;
						worksheet.Cell(index + 1, 14).Value = pneumatici[index - 1].Pneumatici.Veicolo.Targa;
						worksheet.Cell(index + 1, 15).Value = pneumatici[index - 1].Pneumatici.Veicolo.Cliente.Nome + " " + pneumatici[index - 1].Pneumatici.Veicolo.Cliente.Cognome;
						worksheet.Cell(index + 1, 16).Value = pneumatici[index - 1].Pneumatici.Veicolo.Cliente.IsAzienda == true ? "X" : "";
					}
				}
				using (var stream = new MemoryStream())
				{
					workbook.SaveAs(stream);
					var content = stream.ToArray();
					return content;
				}
			}
		}
	}
}
