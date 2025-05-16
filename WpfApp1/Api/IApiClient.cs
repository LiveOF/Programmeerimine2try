using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfApp1.Api
{
    public interface IApiClient
    {
        Task<Result<List<MyObject>>> GetObjectsAsync();
        Task<Result> AddObjectAsync(MyObject newObject);
        Task<Result> SaveObjectAsync(MyObject updatedObject);
        Task<Result> DeleteObjectAsync(int id);
    }
}