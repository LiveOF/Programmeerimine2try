using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KooliProjekt.BlazorApp
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Building methods
        public async Task<Result<List<Building>>> List()
        {
            var result = new Result<List<Building>>();
            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<List<Building>>("Building");
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
                Console.WriteLine($"Error in API List: {ex.Message}");
                return result;
            }
        }

        public async Task<Result<Building>> Get(int id)
        {
            var result = new Result<Building>();
            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<Building>($"Building/{id}");
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
                Console.WriteLine($"Error in API Get: {ex.Message}");
                return result;
            }
        }

        public async Task<Result> Save(Building building)
        {
            var result = new Result();
            try
            {
                HttpResponseMessage response;
                if (building.Id == 0)
                {
                    response = await _httpClient.PostAsJsonAsync("Building", building);
                }
                else
                {
                    response = await _httpClient.PutAsJsonAsync($"Building/{building.Id}", building);
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    result.AddError("_", $"Error: {response.StatusCode} - {errorContent}");
                }

                return result;
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
                return result;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                await _httpClient.DeleteAsync($"Building/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in API Delete: {ex.Message}");
            }
        }

        // Panel methods
        public async Task<Result<List<Panel>>> ListPanels()
        {
            var result = new Result<List<Panel>>();
            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<List<Panel>>("Panel");
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
                Console.WriteLine($"Error in API ListPanels: {ex.Message}");
                return result;
            }
        }

        public async Task<Result<Panel>> GetPanel(int id)
        {
            var result = new Result<Panel>();
            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<Panel>($"Panel/{id}");
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
                Console.WriteLine($"Error in API GetPanel: {ex.Message}");
                return result;
            }
        }

        public async Task<Result> SavePanel(Panel panel)
        {
            var result = new Result();
            try
            {
                HttpResponseMessage response;
                if (panel.Id == 0)
                {
                    response = await _httpClient.PostAsJsonAsync("Panel", panel);
                }
                else
                {
                    response = await _httpClient.PutAsJsonAsync($"Panel/{panel.Id}", panel);
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    result.AddError("_", $"Error: {response.StatusCode} - {errorContent}");
                }

                return result;
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
                return result;
            }
        }

        public async Task DeletePanel(int id)
        {
            try
            {
                await _httpClient.DeleteAsync($"Panel/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in API DeletePanel: {ex.Message}");
            }
        }

        // Material methods
        public async Task<Result<List<Material>>> ListMaterials()
        {
            var result = new Result<List<Material>>();
            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<List<Material>>("Material");
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
                Console.WriteLine($"Error in API ListMaterials: {ex.Message}");
                return result;
            }
        }

        public async Task<Result<Material>> GetMaterial(int id)
        {
            var result = new Result<Material>();
            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<Material>($"Material/{id}");
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
                Console.WriteLine($"Error in API GetMaterial: {ex.Message}");
                return result;
            }
        }

        public async Task<Result> SaveMaterial(Material material)
        {
            var result = new Result();
            try
            {
                HttpResponseMessage response;
                if (material.Id == 0)
                {
                    response = await _httpClient.PostAsJsonAsync("Material", material);
                }
                else
                {
                    response = await _httpClient.PutAsJsonAsync($"Material/{material.Id}", material);
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    result.AddError("_", $"Error: {response.StatusCode} - {errorContent}");
                }

                return result;
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
                return result;
            }
        }

        public async Task DeleteMaterial(int id)
        {
            try
            {
                await _httpClient.DeleteAsync($"Material/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in API DeleteMaterial: {ex.Message}");
            }
        }

        // Service methods
        public async Task<Result<List<Service>>> ListServices()
        {
            var result = new Result<List<Service>>();
            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<List<Service>>("Service");
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
                Console.WriteLine($"Error in API ListServices: {ex.Message}");
                return result;
            }
        }

        public async Task<Result<Service>> GetService(int id)
        {
            var result = new Result<Service>();
            try
            {
                result.Value = await _httpClient.GetFromJsonAsync<Service>($"Service/{id}");
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
                Console.WriteLine($"Error in API GetService: {ex.Message}");
                return result;
            }
        }

        public async Task<Result> SaveService(Service service)
        {
            var result = new Result();
            try
            {
                HttpResponseMessage response;
                if (service.Id == 0)
                {
                    response = await _httpClient.PostAsJsonAsync("Service", service);
                }
                else
                {
                    response = await _httpClient.PutAsJsonAsync($"Service/{service.Id}", service);
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    result.AddError("_", $"Error: {response.StatusCode} - {errorContent}");
                }

                return result;
            }
            catch (Exception ex)
            {
                result.AddError("_", ex.Message);
                return result;
            }
        }

        public async Task DeleteService(int id)
        {
            try
            {
                await _httpClient.DeleteAsync($"Service/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in API DeleteService: {ex.Message}");
            }
        }
    }
}