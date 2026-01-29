using LibraryProject.Application.Dto;
using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Exceptions;
using LibraryProject.Domain.Exceptions.Nonexistent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

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


        public async Task<LoginSession> LoginAsync (Guid userId, string accountName, string password, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(accountName))
            {
                throw new ArgumentException("Username is required.", nameof(accountName));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password is required.", nameof(password));
            }

            Account? account = await _accountRepository.GetAccountByUsernameAsync(accountName, ct);
            if(account == null || account.UserId != userId)
            {
                throw new SecurityException("Invalid credentials.");
            }
            if (!VarifyPassword(password, account.Password))
            {
                throw new SecurityException("Invalid credentials.");
            }
            if (account.IsSuspended)
            {
                throw new SecurityException("Account is suspended.");
            } 
            User? user = await _userRepository.GetExistingUserByIdAsync(userId, ct);
            if (user == null)
            {
                throw new SecurityException("Invalid credentials.");
            }

            if (!user.IsAuthentication)
            {
                throw new SecurityException("User identity is not confirmed.");
            }

            return new LoginSession(user.Id, user.UserType, account.AccountName);
        }

        public async Task<Account> RegisterAccountAsync(string accountName, string password, string email, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(accountName) || accountName.Length > 20)
            {
                throw new ArgumentException("Username is required and must be less than 20 characters.");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                throw new ArgumentException("Password is required and must be at least 8 characters long.");
            }

            User newUser = new User(accountName, Domain.Enum.UserType.Admin, default);
            await _userRepository.SaveUserAsync(newUser);

            User? user = await _userRepository.GetExistingUserByIdAsync(newUser.Id);
            if (user == null)
            {
                throw new NonexistentUserException();
            }

            Account? account = await _accountRepository.GetAccountByUsernameAsync(accountName);
            if (account != null) 
            {
                throw new AccountUsedException(account);
            }

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
                throw new SecurityException($"{interestedAccount.AccountName} has invalid data.");
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
                throw new InvalidOperationException();
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
            Account? account = await _accountRepository.GetAccountByUserIdAsync(_currentUserContext.UserId.Value, ct);
            if (account is null)
            {
                throw new ArgumentNullException(nameof(account));
            }
                
            account.ChangeEmail(newEmail);
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
