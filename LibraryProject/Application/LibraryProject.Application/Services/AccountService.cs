using LibraryProject.Application.Dto;
using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthorizationService _authorizationService;

        public AccountService(IAccountRepository accountRepository, IAuthorizationService authorizationService, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _authorizationService = authorizationService;
            _userRepository = userRepository;
        }


        public LoginSession Login (Guid userId, string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Username is required.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password is required.", nameof(password));
            }

            Account? account = _accountRepository.GetAccountByUsername(name);
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

            User? user = _userRepository.GetExistingUserById(userId);
            if (user == null)
            {
                throw new SecurityException("Invalid credentials.");
            }

            return new LoginSession(user.Id, user.UserType, account.Name);
        }

        public Account RegisterNewAccount(Guid userId, string userName, string password, string email)
        {
     
            if (string.IsNullOrWhiteSpace(userName) || userName.Length > 20)
            {
                throw new ArgumentException("Username is required and must be less than 20 characters.");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                throw new ArgumentException("Password is required and must be at least 8 characters long.");
            }

            User? user = _userRepository.GetExistingUserById(userId);
            if (user == null) {
                throw new NonExistingUserException();
            }
            Account? account = _accountRepository.GetAccountByUsername(userName);
            if (account != null) {
                throw new IsAreadyTakenBySomeoneElseException(account);
            }

            string hashedPassword = HashPassword(password);
            Account newAccount = new Account (user, userName, hashedPassword, email);

            _accountRepository.SaveAccountToStorage(newAccount);
            return newAccount;
        }

        public bool DeleteAccount(int accountId, Guid userId)
        {
            _authorizationService.EnsureAuthenticated();
            Account? interestedAccount = _accountRepository.GetAccountByAccountId(accountId);

            if (interestedAccount != null && interestedAccount.UserId == userId)
            {
                _accountRepository.DeleteAccountFromStorage(interestedAccount);
                return true;
            }
            else 
            {
                return false;
            }

        }

        public bool SuspendAccount(int accountId)
        {
            _authorizationService.EnsureAdmin();

            Account? interestedAccount = _accountRepository.GetAccountByAccountId(accountId);

            if (interestedAccount != null && interestedAccount.CanBeSuspended() )
            {
                interestedAccount.DeactivateAccount();
                return true;
            }
            else 
            {
                return false;
            }
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
