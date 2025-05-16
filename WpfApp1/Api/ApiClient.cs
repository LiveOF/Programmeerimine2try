using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WpfApp1.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client;
        private readonly string _apiBaseUrl;

        public ApiClient(HttpClient client, string apiBaseUrl)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _apiBaseUrl = apiBaseUrl ?? throw new ArgumentNullException(nameof(apiBaseUrl));
        }

        public async Task<Result<List<MyObject>>> GetObjectsAsync()
        {
            try
            {
                var response = await _client.GetStringAsync(_apiBaseUrl);
                var objects = JsonConvert.DeserializeObject<List<MyObject>>(response);
                return Result.Success(objects ?? new List<MyObject>());
            }
            catch (Exception ex)
            {
                return Result.Failure<List<MyObject>>($"Failed to retrieve objects: {ex.Message}");
            }
        }

        public async Task<Result> AddObjectAsync(MyObject newObject)
        {
            try
            {
                var json = JsonConvert.SerializeObject(newObject);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(_apiBaseUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Result.Failure($"Failed to add object. Status: {response.StatusCode}, Details: {errorContent}");
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to add object: {ex.Message}");
            }
        }

        public async Task<Result> SaveObjectAsync(MyObject updatedObject)
        {
            try
            {
                var json = JsonConvert.SerializeObject(updatedObject);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"{_apiBaseUrl}/{updatedObject.Id}", content);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Result.Failure($"Failed to update object. Status: {response.StatusCode}, Details: {errorContent}");
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to update object: {ex.Message}");
            }
        }

        public async Task<Result> DeleteObjectAsync(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"{_apiBaseUrl}/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Result.Failure($"Failed to delete object. Status: {response.StatusCode}, Details: {errorContent}");
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to delete object: {ex.Message}");
            }
        }
    }
}