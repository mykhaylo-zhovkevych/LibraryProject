using LibraryProject.Application.Dto;
using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Domain.Exceptions;
using LibraryProject.Domain.Exceptions.Nonexistent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LibraryProject.Application.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUserContext _currentUserContext;

        public AccountService(IAccountRepository accountRepository, IAuthorizationService authorizationService, IUserRepository userRepository, ICurrentUserContext currentUserContext)
        {
            _accountRepository = accountRepository;
            _authorizationService = authorizationService;
            _userRepository = userRepository;
            _currentUserContext = currentUserContext;
        }

        public async Task<LoginSession> LoginAsync(string accountName, string password, UserType customerType, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(accountName))
            {
                throw new ArgumentException("Benutzername ist erforderlich.", nameof(accountName));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Passwort ist erforderlich.", nameof(password));
            }

            Account? account = await _accountRepository.GetAccountByAccountNameAsync(accountName, ct);
            if (account == null)
            {
                throw new SecurityException("Ungültige Anmeldedaten.");
            }
            if (account.IsSuspended)
            {
                throw new SecurityException("Das Konto ist gesperrt.");
            }

            if (!VarifyPassword(password, account.Password))
            {
                throw new SecurityException("Ungültige Anmeldedaten.");
            }

            User? user = await _userRepository.GetExistingUserByIdAsync(account.UserId, ct);
            if (user == null)
            {
                throw new SecurityException("Ungültige Anmeldedaten.");
            }

            if (user.UserType != customerType)
            {
                throw new SecurityException("Ungültige Anmeldedaten.");
            }

            //if (!user.IsAuthentication)
            //{
            //    throw new SecurityException("Die Benutzeridentität ist nicht bestätigt.");
            //}

            return new LoginSession(user.Id, user.UserType, account.AccountName);
        }

        public async Task<LoginSession> LoginAdminAsync(string accountName, string password, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(accountName))
            {
                throw new ArgumentException("Benutzername ist erforderlich.", nameof(accountName));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Passwort ist erforderlich.", nameof(password));
            }

            Account? account = await _accountRepository.GetAccountByAccountNameAsync(accountName, ct);
            if (account == null)
            {
                throw new SecurityException("Ungültige Anmeldedaten.");
            }

            if (!VarifyPassword(password, account.Password))
            {
                throw new SecurityException("Ungültige Anmeldedaten.");
            }
            if (account.IsSuspended)
            {
                throw new SecurityException("Das Konto ist gesperrt.");
            }

            User? user = await _userRepository.GetExistingUserByIdAsync(account.UserId, ct);
            if (user == null)
            {
                throw new SecurityException("Ungültige Anmeldedaten.");
            }

            if (user.UserType != UserType.Admin)
            {
                throw new SecurityException("Nur Administratoren können sich hier anmelden.");
            }

            //if (!user.IsAuthentication)
            //{
            //    throw new SecurityException("Die Benutzeridentität ist nicht bestätigt.");
            //}

            return new LoginSession(user.Id, user.UserType, account.AccountName);
        }

        public async Task<Account> RegisterAccountAsync(string accountName, string password, string email, string name, string? surname, string address, UserType customerType, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(accountName) || accountName.Length > 20)
            {
                throw new ArgumentException("Benutzername ist erforderlich und muss kürzer als 20 Zeichen sein.");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                throw new ArgumentException("Passwort ist erforderlich und muss mindestens 8 Zeichen lang sein.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name ist erforderlich.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("Adresse ist erforderlich.", nameof(address));
            }

            if (customerType is not (UserType.Student or UserType.Teacher))
            {
                throw new SecurityException("Ungültiger Benutzertyp für die Registrierung.");
            }

            Account? account = await _accountRepository.GetAccountByAccountNameAsync(accountName);
            if (account != null)
            {
                throw new AccountUsedException(account);
            }
            User newUser = new User(name, surname, address, customerType);
            await _userRepository.SaveUserAsync(newUser);

            string hashedPassword = HashPassword(password);
            Account newAccount = new Account(newUser, accountName, hashedPassword, email);

            await _accountRepository.SaveAccountAsync(newAccount);
            return newAccount;
        }

        public async Task<Account> RegisterAdminAccountAsync(string accountName, string password, string email, string name, string? surname, string address, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(accountName) || accountName.Length > 20)
            {
                throw new ArgumentException("Benutzername ist erforderlich und muss kürzer als 20 Zeichen sein.");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                throw new ArgumentException("Passwort ist erforderlich und muss mindestens 8 Zeichen lang sein.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name ist erforderlich.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("Adresse ist erforderlich.", nameof(address));
            }

            Account? existing = await _accountRepository.GetAccountByAccountNameAsync(accountName, ct);
            if (existing != null)
            {
                throw new AccountUsedException(existing);
            }

            User newUser = new User(name, surname, address, UserType.Admin);
            await _userRepository.SaveUserAsync(newUser, ct);

            string hashedPassword = HashPassword(password);
            Account newAccount = new Account(newUser, accountName, hashedPassword, email);

            await _accountRepository.SaveAccountAsync(newAccount);
            return newAccount;
        }

        public async Task DeleteAccountAsync(int accountId, Guid userId, CancellationToken ct)
        {
            _authorizationService.EnsureAuthenticated();
            Account? interestedAccount = await _accountRepository.GetAccountByAccountIdAsync(accountId, ct) ?? throw new NonexistingAccountException();

            if (interestedAccount.UserId != userId)
            {
                throw new SecurityException($"{interestedAccount.AccountName} hat ungültige Daten.");
            }

            await _accountRepository.DeleteAccountAsync(interestedAccount, ct);
        }

        public async Task<Account?> ReceiveAccountByUserIdAsync(Guid userId, CancellationToken ct)
        {
            _authorizationService.EnsureAuthenticated();
            return await _accountRepository.GetAccountByUserIdAsync(userId, ct);
        }

        public async Task SuspendAccountAsync(int accountId, CancellationToken ct)
        {
            _authorizationService.EnsureAdmin();
            Account? interestedAccount = await _accountRepository.GetAccountByAccountIdAsync(accountId, ct) ?? throw new NonexistingAccountException();

            if (interestedAccount.CanBeSuspended())
            {
                interestedAccount.DeactivateAccount();
                await _accountRepository.UpdateAccountAsync(interestedAccount, ct);
            }
            else
            {
                throw new InvalidOperationException("Das Konto kann derzeit nicht gesperrt werden.");
            }
        }

        public async Task ReactivateAccountAsync(int accountId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _authorizationService.EnsureAdmin();

            Account? interestedAccount = await _accountRepository.GetAccountByAccountIdAsync(accountId, ct) ?? throw new NonexistingAccountException();

            if (!interestedAccount.IsSuspended) return;

            interestedAccount.ReactivateAccount();
            await _accountRepository.UpdateAccountAsync(interestedAccount, ct);
        }

        public async Task<List<Account>> ReceiveAllAccountsAsync(CancellationToken ct = default)
        {
            _authorizationService.EnsureAdmin();
            return await _accountRepository.GetAllAccountsAsync(ct);
        }

        public async Task UpdateEmailAsync(string newEmail, CancellationToken ct = default)
        {
            _authorizationService.EnsureAuthenticated();
            if (string.IsNullOrWhiteSpace(newEmail))
            {
                throw new ArgumentException("E-Mail-Adresse ist erforderlich.", nameof(newEmail));
            }
            if (!newEmail.Contains('@'))
            {
                throw new ArgumentException("Ungültige E-Mail-Adresse.", nameof(newEmail));
            }

            Account? account = await _accountRepository.GetAccountByUserIdAsync(_currentUserContext.UserId.Value, ct);
            if (account is null)
            {
                throw new ArgumentNullException(nameof(account), "Konto wurde nicht gefunden.");
            }

            account.ChangeEmail(newEmail);
            await _accountRepository.UpdateAccountAsync(account, ct);
        }


        public async Task UpdateAccountNameAsync(string newAccountName, CancellationToken ct = default)
        {
            _authorizationService.EnsureAuthenticated();
            Account? account = await _accountRepository.GetAccountByUserIdAsync(_currentUserContext.UserId.Value, ct);
            if (account is null)
            {
                throw new ArgumentNullException(nameof(account), "Konto wurde nicht gefunden.");
            }
            account.ChangeAccountName(newAccountName);
            await _accountRepository.UpdateAccountAsync(account, ct);
        }

        private static string HashPassword(string password)
        {
            const int saltSize = 16; // 128-bit
            const int keySize = 32; // 256-bit
            const int iterations = 100_000;

            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);

            byte[] key = Rfc2898DeriveBytes.Pbkdf2
                (
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                keySize
                );
            // Formating
            return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
        }

        private static bool VarifyPassword(string password, string stored)
        {
            string[] parts = stored.Split('.', 3);
            if (parts.Length != 3)
            {
                return false;
            }

            if (!int.TryParse(parts[0], out int iterations))
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] expectedKey = Convert.FromBase64String(parts[2]);

            byte[] actualKey = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expectedKey.Length);

            return CryptographicOperations.FixedTimeEquals(actualKey, expectedKey);
        }
    }
}
