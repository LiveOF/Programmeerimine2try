using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.WpfApp.Api
{
    public interface IApiClient
    {
        Task<IList<Building>> List();
        Task<Result> Save(Building building);
        Task<Result> Delete(int id);
    }
}
