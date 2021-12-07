using SouthernCross.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SouthernCross.Core.Services
{
    public interface IService<T> where T : BaseEntity
    {
        Task<T> GetAsync(int Id);
        Task<IList<T>> GetAsync();
    }
}
