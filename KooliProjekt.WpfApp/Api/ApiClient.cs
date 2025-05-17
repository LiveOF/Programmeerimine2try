using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KooliProjekt.WpfApp.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<IList<Building>> List()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Building>>("api/Buildings");
            return response;
        }

        public async Task<Result> Save(Building building)
        {
            try
            {
                HttpResponseMessage response;
                if (building.Id == 0)
                {
                    response = await _httpClient.PostAsJsonAsync("api/Buildings", building);
                }
                else
                {
                    response = await _httpClient.PutAsJsonAsync($"api/Buildings/{building.Id}", building);
                }

                if (response.IsSuccessStatusCode)
                {
                    return Result.Success();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return Result.Failure(error);
                }
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Buildings/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return Result.Success();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return Result.Failure(error);
                }
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
