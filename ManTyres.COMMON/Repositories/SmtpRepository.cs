using ManTyres.COMMON.DTO;
using ManTyres.COMMON.Interfaces;

namespace ManTyres.COMMON.Repositories
{
	public class SmtpRepository : ISmtpRepository
	{
		public string VerificationCodeEmail(UserDTO sendRequest, string verificationCode)
		{
			return @$"
            <div>
               <div style='font-family:Roboto, RobotoDraft, Helvetica, Arial, sans-serif;font-size:14px;width:100%;background:#fafafa;padding:40px 0 30px;'>
                  <div style='width:600px;max-width:100%;background:#fff;border-radius:6px;margin:0 auto;'>
                     <img width='100%' src='https://mgcrmbotaee8.blob.core.windows.net/teq-link/logo.png' style='background:#000;'>
                     <div style='padding:16px 40px 40px;'>
                        <h3 style='font-weight:bold;font-size:24px;line-height:36px;margin:16px 0;color:#1e2026;'>
                           {AuthorizeTechnician(sendRequest.CultureInfo)}
                        </h3>
                        <div style='font-family:IBM Plex Sans;margin:0;color:#474d57;font-size:16px;'>
                           <p style='margin-bottom:26px;'>
                              {sendRequest.UserName}&nbsp;{Header(sendRequest.CultureInfo, sendRequest.Email)}
                           </p>
                        </div>
                        <div style='padding:16px;margin:12px 0 32px;background-color:#fafafa;border-radius:4px;'>
                           <table class='yiv4202240446deviceInfo'
                              style='width:100%;background:#fafafa;vertical-align:middle;padding:10px 0 10px;'>
                              <tbody>
                                 <tr>
                                    <td>
                                       <table>
                                          <tbody>
                                             <tr>
                                                <td
                                                   style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                                   Email: </td>
                                                <td>
                                                   <div>
                                                      <div style='color:#474d57;'>{sendRequest.Email}</div>
                                                   </div>
                                                </td>
                                             </tr>
                                             <tr>
                                                <td
                                                   style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                                   {Phone(sendRequest.CultureInfo)}: </td>
                                                <td>
                                                   <div>
                                                      <div style='color:#474d57;'>{sendRequest.PhoneNumber}</div>
                                                   </div>
                                                </td>
                                             </tr>
                                             <tr>
                                                <td
                                                   style='vertical-align:text-top;text-align:left;margin-right:3px;color:#76808f;white-space:nowrap;text-align:right;font-size:14px;padding-right:8px;padding-left:8px;'>
                                                   {Equipment(sendRequest.CultureInfo)}: </td>
                                                <td>
                                                   <div>
                                                      <div style='color:#474d57;'>{sendRequest.CompanyName}
                                                         {sendRequest.Email} ({sendRequest.PhoneNumber})
                                                      </div>
                                                   </div>
                                                </td>
                                             </tr>
                                          </tbody>
                                       </table>
                                    </td>
                                 </tr>
                              </tbody>
                           </table>
                        </div>
                        <p class='yiv4202240446section' style='font-weight:normal;margin:16px 0 0 0;color:#474d57;'>
                           {Action(sendRequest.CultureInfo)}
                        </p>
                        <div class='yiv4202240446bigVertifyCode' style='font-size:32px;margin-top:16px;color:#1e2026;'>{verificationCode}
                        </div>
                        <div style='font-family:IBM Plex Sans;line-height:24px;color:#76808f;margin-top: 36px;'>
                           Teq-Link Team
                           <br>
                           {Footer(sendRequest.CultureInfo)}
                        </div>
                     </div>
                  </div>
                  <div style='text-align:center;'>
                     <p style='font-family:IBM Plex Sans;color:#aeb4bc !important;margin:0;'>
                        <br>
                        2021 © Teq-Link.com All Rights Reserved
                        <br>
                        URL：www.teq-link.com <br>
                        E-mail：support@teq-link.com
                     </p>
                  </div>
               </div>
            </div>";
		}

		public string AuthorizeTechnician(string cultureInfo)
		{
			switch (cultureInfo)
			{
				case "de":
					return "Autorisierter Techniker";
				case "es":
					return "Técnico autorizado";
				case "fr":
					return "Technicien autorisé";
				case "it":
					return "Tecnico autorizzato";
				default:
					return "Authorize Technician";
			}
		}
		public string Header(string cultureInfo, string SerialNumber)
		{
			switch (cultureInfo)
			{
				case "de":
					return $"möchte die Maschine {SerialNumber} seiner Liste zuordnen. Aus Sicherheitsgründen müssen Sie dem Techniker den Bestätigungscode mitteilen, um die Zuordnung zu ermöglichen.";
				case "es":
					return $"quiere asociar la máquina {SerialNumber} a su lista. Como medida de seguridad, requerimos que comunique el código de verificación al técnico para permitir la asociación.";
				case "fr":
					return $"veut associer la machine {SerialNumber} à sa liste. Par mesure de sécurité, nous vous demandons de communiquer le code de vérification au technicien pour permettre l'association.";
				case "it":
					return $"vuole associare la macchina {SerialNumber} alla sua lista. Come misura di sicurezza, ti chiediamo di comunicare il codice di verifica al tecnico per consentire l'associazione.";
				default:
					return $"wants to associate the machine {SerialNumber} to its list. As a security measure, we require you to communicate the verification code to the technician to allow the association.";
			}
		}
		public string Phone(string cultureInfo)
		{
			switch (cultureInfo)
			{
				case "de":
					return "Telefon";
				case "es":
					return "Teléfono";
				case "fr":
					return "Téléphone";
				case "it":
					return "Telefono";
				default:
					return "Phone";
			}
		}
		public string Equipment(string cultureInfo)
		{
			switch (cultureInfo)
			{
				case "de":
					return "Ausrüstung";
				case "es":
					return "Equipo";
				case "fr":
					return "Équipement";
				case "it":
					return "Attrezzatura";
				default:
					return "Equipment";
			}
		}

		public string Action(string cultureInfo)
		{
			switch (cultureInfo)
			{
				case "de":
					return "Wenn Sie diese Aktivität erkennen, teilen Sie dem Techniker bitte den Aktivierungscode mit. Hier ist der Aktivierungscode:";
				case "es":
					return "Si reconoce esta actividad, comunique el código de activación al técnico. Aquí está el código de activación:";
				case "fr":
					return "Si vous reconnaissez cette activité, veuillez communiquer le code d'activation au technicien. Voici le code d'activation :";
				case "it":
					return "Se riconosci questa attività, comunica il codice di attivazione al tecnico. Ecco il codice di attivazione:";
				default:
					return "If you recognize this activity, please communicate the activation code to the technician. Here is the activation code:";
			}
		}

		public string Footer(string cultureInfo)
		{
			switch (cultureInfo)
			{
				case "de":
					return "Dies ist eine automatisierte Nachricht, bitte antworte nicht.";
				case "es":
					return "Este es un mensaje automático, no responda.";
				case "fr":
					return "Ceci est un message automatique, merci de ne pas répondre.";
				case "it":
					return "Questo è un messaggio automatico, per favore non rispondere.";
				default:
					return "This is an automated message, please do not reply.";
			}
		}
	}
}
