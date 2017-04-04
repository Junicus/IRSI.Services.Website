using IRSI.Services.Website.Configuration;
using IRSI.Services.Website.Models.Common;
using IRSI.Services.Website.Models.SOS;
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
    public class SOSApiClient : HttpClient
    {
        public SOSApiClient(IOptions<SOSApiClientOptions> options)
        {
            BaseAddress = new Uri(options.Value.ApiUrl);
        }

        public void SetAccessToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken)) throw new ArgumentNullException("accessToken");
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<List<Store>> GetStoresAsync()
        {
            var storesResult = new List<Store>();
            var result = await GetAsync("/api/sos/stores");
            if (result.IsSuccessStatusCode)
            {
                var storesJson = await result.Content.ReadAsStringAsync();

                var resultModel = JsonConvert.DeserializeObject<StoresResultModel>(storesJson);
                foreach (var store in resultModel.Stores)
                {
                    storesResult.Add(store);
                }
            }

            return storesResult;
        }

        public async Task<Store> GetStoreAsync(Guid storeId)
        {
            var result = await GetAsync($"/api/sos/stores/{storeId}");
            if (result.IsSuccessStatusCode)
            {
                var storeJson = await result.Content.ReadAsStringAsync();
                var store = JsonConvert.DeserializeObject<Store>(storeJson);
                return store;
            }

            return null;
        }


        public async Task<List<KpiModel>> GetKpisAsync(Store store, DateTime startDate, DateTime endDate)
        {
            var kpiResults = new List<KpiModel>();
            var startDateFixed = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var endDateFixed = new DateTime(endDate.Year, endDate.Month, endDate.Day);

            var startDateEncoded = Uri.EscapeDataString(startDateFixed.ToString());
            var endDateEncoded = Uri.EscapeDataString(endDateFixed.ToString());

            var result = await GetAsync($"/api/sos/stores/{store.Id}/kpi?startDate={startDateEncoded}&endDate={endDateEncoded}");
            if (result.IsSuccessStatusCode)
            {
                var kpiJson = await result.Content.ReadAsStringAsync();
                var kpis = JsonConvert.DeserializeObject<List<KpiModel>>(kpiJson);
                foreach (var kpi in kpis)
                {
                    kpiResults.Add(kpi);
                }
            }

            return kpiResults;
        }

        public async Task<List<SummaryDayPartGroupModel>> GetSummariesAsync(Store store, DateTime startDate, DateTime endDate)
        {
            var summaryResults = new List<SummaryDayPartGroupModel>();
            var startDateFixed = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var endDateFixed = new DateTime(endDate.Year, endDate.Month, endDate.Day);

            var startDateEncoded = Uri.EscapeDataString(startDateFixed.ToString());
            var endDateEncoded = Uri.EscapeDataString(endDateFixed.ToString());

            var result = await GetAsync($"/api/sos/stores/{store.Id}/summary?startDate={startDateEncoded}&endDate={endDateEncoded}");
            if (result.IsSuccessStatusCode)
            {
                var summariesJson = await result.Content.ReadAsStringAsync();
                var summaries = JsonConvert.DeserializeObject<List<SummaryDayPartGroupModel>>(summariesJson);
                foreach (var summary in summaries)
                {
                    summaryResults.Add(summary);
                }
            }

            return summaryResults;
        }
    }
}
