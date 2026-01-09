using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Interfaces
{
    public interface IPolicyRepository
    {
        void SavePolicy(UserType userType, ItemType itemType, Policy policy);
        void RemovePolicy(UserType userType, ItemType itemType);
        Policy? GetPolicy(UserType userType, ItemType itemType);
    }
}
