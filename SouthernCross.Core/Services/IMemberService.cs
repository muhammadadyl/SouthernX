using SouthernCross.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthernCross.Core.Services
{
    public interface IMemberService : IService<Member>
    {
        Task<List<Member>> GetAsync(string policyNumber, string memberCardNumber);

        Task<List<Member>> GetAsync(DateTime dateOfBith);
    }
}
