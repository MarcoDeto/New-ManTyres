using ManTyres.COMMON.DTO;

namespace ManTyres.COMMON.Interfaces
{
	public interface ISmtpRepository
	{
		string VerificationCodeEmail(UserDTO sendRequest, string verificationCode);
	}
}
