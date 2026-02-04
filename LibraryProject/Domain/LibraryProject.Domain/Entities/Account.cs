using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Entities
{
    public class Account
    {
        public int AccountId { get; private set; }
        public Guid UserId { get; private set; }
        public string AccountName { get; private set; }
        public string Password { get; private set; }
        public string? Email { get; private set; }
        public bool IsSuspended { get; private set; } = false;

        protected Account() { }

        public Account(User userId, string accountname, string password, string? email = null)
        {
            AccountId = new Random().Next(1, int.MaxValue);
            UserId = userId.Id;
            AccountName = accountname;
            Password = password;
            Email = email;
            IsSuspended = false;
        }

        public void ChangeEmail(string selectedEmail)
        {
            Email = selectedEmail;
        }

        public void ChangeAccountName(string selectedAccountName)
        {
            AccountName = selectedAccountName;
        }

        public bool CanBeSuspended()
        {
            if (IsSuspended)
            return false;

            return true;
        }

        public void ReactivateAccount()
        {
            IsSuspended = false;
        }

        public void DeactivateAccount()
        {
            IsSuspended = true;
        }
    }   
}