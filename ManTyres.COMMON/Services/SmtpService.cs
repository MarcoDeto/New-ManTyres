using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using ManTyres.COMMON.DTO;
using ManTyres.COMMON.Interfaces;
using ManTyres.COMMON.Utils;

namespace ManTyres.COMMON.Services
{
	public class SmtpService : BaseService, ISmtpService
	{
		private readonly ISmtpRepository _Emails;
		private readonly IConfiguration _configuration;
		private readonly SendGridClient _client;
		public SmtpService(ISmtpRepository emails, IConfiguration configuration)
		{
			_configuration = configuration;
			_Emails = emails;
			_client = new SendGridClient(_configuration.GetSection("SENDGRID_API_KEY").Value);
		}

		public async Task<string> EquipmentTechnicalAssociation(UserDTO sendRequest)
		{
			string verificationCode = _generateVerificationCode();
			string body = _Emails.VerificationCodeEmail(sendRequest, verificationCode);

			/*var from = new EmailAddress(sendRequest.emailSender);
			var to = new EmailAddress(sendRequest.recipientEmail);

			try
			{
				var message = MailHelper.CreateSingleEmail(from, to, "verification Code", body, body);
				var response = await _client.SendEmailAsync(message).ConfigureAwait(false);
				if (response.IsSuccessStatusCode)
					return verificationCode;
				else
				{
					var res = await response.Body.ReadAsStringAsync();
					throw new Exception(res);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}*/
			return null;
		}

		public async Task<bool> BackOfficeMail(BackOfficeMailDTO mailDTO)
		{
			var from = new EmailAddress(mailDTO.emailSender);
			var to = new EmailAddress(mailDTO.recipientEmail);

			try
			{
				var message = MailHelper.CreateSingleEmail(from, to, mailDTO.emailSubject, mailDTO.emailBody, mailDTO.emailBody);
				var response = await _client.SendEmailAsync(message);
				if (response.IsSuccessStatusCode)
					return true;
				else
				{
					var res = await response.Body.ReadAsStringAsync();
					throw new Exception(res);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public BackOfficeMailDTO ClaimEmailGenerator(BackOfficeMailDTO claimEmail, ClaimDTO claimDTO, string note = null)
		{

			claimEmail.emailSender = _configuration.GetSection("EmailVSG:Sender").Value;

			switch (claimDTO.Status)
			{
				case ClaimStatus.Open:
					claimEmail.emailSubject = $"New claim opened: claim n.{claimDTO.Id}.";
					var emailRecipient = _configuration.GetSection("EmailVSG:Recipient").Value;
					claimEmail.recipientEmail = emailRecipient;
					claimEmail.emailBody = Templates.Templates.ClaimStatusOpen();
					break;

				case ClaimStatus.Working:
					claimEmail.emailSubject = $"Claim n.{claimDTO.Id} is being processed.";
					claimEmail.recipientEmail = "marco.gammella@devlabtech.it"; // => Notifica email VSG
																									// TODO: impostare mail dall'appsettings se il passagio è da OPEN a WORKING bisogna inviare la mail TE/DI se il passagio è WORKING INFO --> WORKING viene notificato VSG
					claimEmail.emailBody = Templates.Templates.ClaimStatusWorking();
					break;

				case ClaimStatus.WorkingReqInfo:
					claimEmail.emailSubject = $"Claim n.{claimDTO.Id} is being processed.";
					claimEmail.recipientEmail = claimDTO.ContactEmail;
					claimEmail.emailBody = Templates.Templates.ClaimStatusWorkingReqInfo();
					break;

				case ClaimStatus.ClosedRefused:
					claimEmail.emailSubject = $"Claim n.{claimDTO.Id} has been refused.";
					claimEmail.recipientEmail = claimDTO.ContactEmail;
					claimEmail.emailBody = Templates.Templates.ClaimStatusClosedRefused();
					break;

				case ClaimStatus.ClosedApproved:
					claimEmail.emailSubject = $"Claim n.{claimDTO.Id} has been approved.";
					claimEmail.recipientEmail = claimDTO.ContactEmail;
					claimEmail.emailBody = Templates.Templates.ClaimStatusClosedApproved();
					break;
				default:
					throw new InvalidOperationException("An error occurred while building claim-related email.");
			}
			claimEmail.emailBody = _replaceEmailDefaultValues(claimEmail.emailBody, claimDTO, note);
			return claimEmail;
		}

		private string _generateVerificationCode()
		{
			Random random = new Random();
			int result = random.Next(0, 999999);
			return result.ToString("000000");
		}

		private string _replaceEmailDefaultValues(string claimEmail, ClaimDTO claimDTO, string? note = null)
		{

			claimEmail = claimEmail.Replace("__CLAIMNUMBER__", claimDTO.Id.ToString());
			claimEmail = claimEmail.Replace("__CLAIMSTATUS__", claimDTO.Status.ToString().ToLower());
			claimEmail = claimEmail.Replace("__INSTALLATIONDATE__", claimDTO.InstallationDate.ToString());
			claimEmail = claimEmail.Replace("__FAILUREDATE__", claimDTO.FailureDate.ToString());
			claimEmail = claimEmail.Replace("__CUSTOMERCLAIMCODE__", claimDTO.CustomerClaimCode);
			claimEmail = claimEmail.Replace("__DEFECT__", claimDTO.Defect);
			claimEmail = claimEmail.Replace("__DEFECTDESCRIPTION__", claimDTO.DefectDescription);
			claimEmail = claimEmail.Replace("__REQUIREDINFO__", note);
			claimEmail = claimEmail.Replace("__CONTACTPERSON__", claimDTO.ContactPerson);
			claimEmail = claimEmail.Replace("__CONTACTPHONE__", claimDTO.ContactPhone);
			claimEmail = claimEmail.Replace("__CONTACTEMAIL__", claimDTO.ContactEmail);

			return claimEmail;
		}

		public async Task<bool> ForceChangePasswordMail(BackOfficeMailDTO mailDTO, string password, string businessName, string personalizedText = "In order to activate your account, login and password change are required.")
		{

			var from = new EmailAddress(mailDTO.emailSender);
			var to = new EmailAddress(mailDTO.recipientEmail);
			mailDTO.emailSubject = Templates.Templates.ResetPasswordSubject(mailDTO.cultureInfo);
			mailDTO.emailBody = Templates.Templates.ResetPasswordBody(mailDTO.cultureInfo, mailDTO.recipientEmail, password, businessName, personalizedText);
			try
			{
				var message = MailHelper.CreateSingleEmail(from, to, mailDTO.emailSubject, mailDTO.emailBody, mailDTO.emailBody);
				var response = await _client.SendEmailAsync(message);
				if (response.IsSuccessStatusCode)
					return true;
				else
				{
					var res = await response.Body.ReadAsStringAsync();
					throw new Exception(res);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

	}
}