namespace ManTyres.COMMON.DTO
{
#nullable disable
	public class SmtpDTO
	{
		public string recipientEmail { get; set; }
		public string emailSender { get; set; }
		public string cultureInfo { get; set; }
	}

	public class EquipmentTechnicalAssociationDTO : SmtpDTO
	{
	}

	public class BackOfficeMailDTO : SmtpDTO
	{
		public string emailSubject { get; set; }
		public string emailBody { get; set; }
	}
}
