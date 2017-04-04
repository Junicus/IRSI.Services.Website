using IRSI.Services.Website.Configuration;
using IRSI.Services.Website.Models.Common;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IRSI.Services.Website.ApiClients
{
    public class StoresApiClient : HttpClient
    {
        public StoresApiClient(IOptions<StoreApiClientOptions> options)
        {
            BaseAddress = new Uri(options.Value.ApiUrl);
        }

        public void SetAccessToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken)) { throw new ArgumentNullException("accessToken"); }
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<IEnumerable<Store>> GetStoresAsync()
        {
            var storesResult = new List<Store>();
            var result = await GetAsync("/api/stores");
            if (result.IsSuccessStatusCode)
            {
                var storesJson = await result.Content.ReadAsStringAsync();
                var resultModel = JsonConvert.DeserializeObject<List<Store>>(storesJson);
                foreach (var store in resultModel)
                {
                    storesResult.Add(store);
                }
            }

            return storesResult;
        }
    }
}
