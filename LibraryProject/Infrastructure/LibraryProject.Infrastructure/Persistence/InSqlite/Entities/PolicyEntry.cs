using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Persistence.InSqlite.Entities
{
    public class PolicyEntry
    {
        public Guid Id { get; set; }
        public UserType UserType { get; set; }
        public ItemType ItemType { get; set; }

        public string? PolicyName { get; set; }
        public uint Extensions { get; set; }
        public decimal LoanFees { get; set; }
        public uint LoadPeriodInDays { get; set; }

    }
}
