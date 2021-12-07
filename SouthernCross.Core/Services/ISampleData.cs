using SouthernCross.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SouthernCross.Core.Services
{
    public interface ISampleData<T> where T : BaseEntity
    {
        Task LoadDataAsync();
    }
}
