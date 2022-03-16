using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManTyres.DAL.SQLServer
{
	public class ItalianIdentityErrorDescriber : IdentityErrorDescriber
	{
		public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = $"Si è verificato un errore sconosciuto." }; }
		public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Errore di concorrenza ottimistica, l'oggetto è stato modificato." }; }
		public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = "Password errata." }; }
		public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = "Token non valido." }; }
		public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Esiste già un utente con queste credenziali." }; }
		public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = $"Il nome utente '{userName}' non è valido, può contenere solo lettere o cifre." }; }
		public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = $"Email '{email}' non valido." }; }
		public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = $"Il nome utente '{userName}' è già in uso." }; }
		public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = $"Email '{email}' è già in uso." }; }
		public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = $"Il nome del ruolo '{role}' non è valido." }; }
		public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = $"Il nome del ruolo '{role}' è già in uso." }; }
		public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "L'utente ha già una password impostata." }; }
		public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "Il blocco non è abilitato per questo utente." }; }
		public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"Utente già nel ruolo '{role}'." }; }
		public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = $"Utente non è nel ruolo '{role}'." }; }
		public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = $"La password devono contenere almeno {length} caratteri." }; }
		public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "La password devono contenere almeno un carattere non alfanumerico." }; }
		public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "La password devono contenere almeno una cifra ('0'-'9')." }; }
		public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "La password devono contenere almeno una minuscola ('a'-'z')." }; }
		public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "La password devono contenere almeno una maiuscola ('A'-'Z')." }; }
		public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) { return new IdentityError { Code = nameof(PasswordRequiresUniqueChars), Description = $"La password devono contenere almeno {uniqueChars} caratteri diversi." }; }
		public override IdentityError RecoveryCodeRedemptionFailed() { return new IdentityError { Code = nameof(RecoveryCodeRedemptionFailed), Description = "il codice di ripristino non è stato riscattato." }; }
	}
}
