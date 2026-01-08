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
        public string Name { get; private set; }
        public string Password { get; private set; }
        public string? Email { get; private set; } = string.Empty;
        public bool IsSuspended { get; private set; } = false;


        public Account(User userId, string name, string password, string? email = null)
        {
            AccountId = new Random().Next(1, int.MaxValue);
            UserId = userId.Id;
            Name = name;
            Password = password;
            Email = email;
            IsSuspended = true;
        }

        public bool CanBeSuspended()
        {
            if (IsSuspended)
            return false;

            return true;
        }

        public void ReactivateAccount()
        {
            IsSuspended = true;
        }

        public void DeactivateAccount()
        {
            IsSuspended = false;
        }



    }   
}