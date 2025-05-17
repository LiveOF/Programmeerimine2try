using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.BlazorApp
{
    public interface IApiClient
    {
        // Здания
        Task<Result<List<Building>>> List();
        Task<Result<Building>> Get(int id);
        Task<Result> Save(Building building);
        Task Delete(int id);

        // Панели
        Task<Result<List<Panel>>> ListPanels();
        Task<Result<Panel>> GetPanel(int id);
        Task<Result> SavePanel(Panel panel);
        Task DeletePanel(int id);

        // Материалы
        Task<Result<List<Material>>> ListMaterials();
        Task<Result<Material>> GetMaterial(int id);
        Task<Result> SaveMaterial(Material material);
        Task DeleteMaterial(int id);

        // Услуги
        Task<Result<List<Service>>> ListServices();
        Task<Result<Service>> GetService(int id);
        Task<Result> SaveService(Service service);
        Task DeleteService(int id);
    }
}

