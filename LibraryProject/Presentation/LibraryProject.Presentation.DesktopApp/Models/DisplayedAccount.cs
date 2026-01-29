using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.Models
{
    public class DisplayedAccount
    {
        public int AccountId { get; }
        public Guid UserId { get; }
        public string AccountName { get; }
        public string Email { get; }
        public string Status { get; }

        public DisplayedAccount(int accountId, Guid userId, string accountName, string email, string status)
        {
            AccountId = accountId;
            UserId = userId;
            AccountName = accountName;
            Email = email;
            Status = status;

        }
    }
}
