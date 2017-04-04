using IRSI.Services.Website.Configuration;
using IRSI.Services.Website.Models.AVT;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IRSI.Services.Website.ApiClients
{
    public class AVTApiClient : HttpClient
    {
        public AVTApiClient(IOptions<AVTApiClientOptions> options)
        {
            BaseAddress = new Uri(options.Value.ApiUrl);
        }

        public void SetAccessToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken)) { throw new ArgumentNullException("accessToken"); }
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<AVTPaged> GetAVTAsync(int page, int page_size)
        {
            var response = await GetAsync($"api/avt?page={page}&page_size={page_size}");
            if (response.IsSuccessStatusCode)
            {
                var avtJson = await response.Content.ReadAsStringAsync();
                var avt = JsonConvert.DeserializeObject<AVTPaged>(avtJson);
                return avt;
            }
            else
            {
                return null;
            }
        }

        public async Task<AVTItem> GetAVTItemAsync(Guid id)
        {
            var response = await GetAsync($"api/avt/{id}");
            if (response.IsSuccessStatusCode)
            {
                var avtJson = await response.Content.ReadAsStringAsync();
                var avt = JsonConvert.DeserializeObject<AVTItem>(avtJson);
                return avt;
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateAVTAsync(Guid id, AVTItem item)
        {
            var avtJson = JsonConvert.SerializeObject(item);
            var response = await PutAsync($"api/avt/{id}", new StringContent(avtJson, Encoding.UTF8, "application/json"));
        }

        public async Task PostAVTAsync(AVTItem item)
        {
            var avtJson = JsonConvert.SerializeObject(item);
            var response = await PostAsync($"api/avt", new StringContent(avtJson, Encoding.UTF8, "application/json"));
        }

        public async Task DeleteAVTAsync(Guid id)
        {
            var response = await DeleteAsync($"api/avt/{id}");
        }
    }
}
