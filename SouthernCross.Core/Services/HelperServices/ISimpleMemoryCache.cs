using System;
using System.Threading.Tasks;

namespace SouthernCross.Core.Services.HelperServices
{
    public interface ISimpleMemoryCache
    {
        Task<T> GetOrCreate<T>(object key, Func<Task<T>> item, int slidingExpiryInMin = 60, int absoluteExpiryInHours = 24);
    }
}
