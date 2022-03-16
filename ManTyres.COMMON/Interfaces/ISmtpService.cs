using ManTyres.COMMON.DTO;
using System.Threading.Tasks;

namespace ManTyres.COMMON.Interfaces
{
	public interface ISmtpService
	{
		Task<string> EquipmentTechnicalAssociation(UserDTO sendRequest);
		Task<bool> BackOfficeMail(BackOfficeMailDTO mailDTO);
		BackOfficeMailDTO ClaimEmailGenerator(BackOfficeMailDTO claimEmail, ClaimDTO claimDTO, string note = null);
		Task<bool> ForceChangePasswordMail(BackOfficeMailDTO mailDTO, string password, string businessName, string personalizedText = "");
	}
}