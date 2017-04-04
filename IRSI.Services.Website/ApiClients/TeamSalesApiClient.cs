using IRSI.Services.Website.Configuration;
using IRSI.Services.Website.Models.Common;
using IRSI.Services.Website.Models.TeamSales;
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
    public class TeamSalesApiClient : HttpClient
    {
        public TeamSalesApiClient(IOptions<TeamSalesApiClientOptions> options)
        {
            BaseAddress = new Uri(options.Value.ApiUrl);
        }

        public void SetAccessToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken)) { throw new ArgumentNullException("accessToken"); }
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<Store> GetStoreAsync(Guid storeId)
        {
            var response = await GetAsync($"api/teamsales/store/{storeId}");
            if (response.IsSuccessStatusCode)
            {
                var storeJson = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(storeJson))
                {
                    return null;
                }
                var store = JsonConvert.DeserializeObject<Store>(storeJson);
                return store;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<EmployeeSalesPerHour>> GetTopEmployeeSalesPerHourAsync(string concept, int storeId, DateTime startDate, DateTime endDate)
        {
            //http://localhost:51000/api/cgb/teamsales/empsalesperhour?storeId=3&startDate=2016-8-10&endDate=2016-8-16&filter=top
            var response = await GetAsync($"api/{concept}/teamsales/empsalesperhour?storeId={storeId}&startDate={startDate.ToString("yyyy-MM-dd")}&endDate={endDate.ToString("yyyy-MM-dd")}");
            if (response.IsSuccessStatusCode)
            {
                var empSalesJson = await response.Content.ReadAsStringAsync();
                var empSales = JsonConvert.DeserializeObject<List<EmployeeSalesPerHour>>(empSalesJson);
                return empSales;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<EmployeeSalesPerHour>> GetBottomEmployeeSalesPerHourAsync(string concept, int storeId, DateTime startDate, DateTime endDate)
        {
            //http://localhost:51000/api/cgb/teamsales/empsalesperhour?storeId=3&startDate=2016-8-10&endDate=2016-8-16&filter=bottom
            var response = await GetAsync($"api/{concept}/teamsales/empsalesperhour?storeId={storeId}&startDate={startDate.ToString("yyyy-MM-dd")}&endDate={endDate.ToString("yyyy-MM-dd")}&filter=bottom");
            if (response.IsSuccessStatusCode)
            {
                var empSalesJson = await response.Content.ReadAsStringAsync();
                var empSales = JsonConvert.DeserializeObject<List<EmployeeSalesPerHour>>(empSalesJson);
                return empSales;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<EmployeeSalesPerHourIncrement>> GetEmployeeSalesPerHourImproveWeekAsync(string concept, int storeId, DateTime startDate, DateTime endDate)
        {
            //http://localhost:51000/api/cgb/teamsales/empsalesperhourimprove?storeId=3&startDate=2016-7-4&endDate=2016-7-31&filter=week
            var response = await GetAsync($"api/{concept}/teamsales/empsalesperhourimprove?storeId={storeId}&startDate={startDate.ToString("yyyy-MM-dd")}&endDate={endDate.ToString("yyyy-MM-dd")}&filter=week");
            if (response.IsSuccessStatusCode)
            {
                var empSalesJson = await response.Content.ReadAsStringAsync();
                var empSales = JsonConvert.DeserializeObject<List<EmployeeSalesPerHourIncrement>>(empSalesJson);
                return empSales;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<EmployeeSalesPerHourIncrement>> GetEmployeeSalesPerHourImprovePeriodAsync(string concept, int storeId, DateTime startDate, DateTime endDate)
        {
            //http://localhost:51000/api/cgb/teamsales/empsalesperhourimprove?storeId=3&startDate=2016-7-4&endDate=2016-7-31&filter=period
            var response = await GetAsync($"api/{concept}/teamsales/empsalesperhourimprove?storeId={storeId}&startDate={startDate.ToString("yyyy-MM-dd")}&endDate={endDate.ToString("yyyy-MM-dd")}&filter=period");
            if (response.IsSuccessStatusCode)
            {
                var empSalesJson = await response.Content.ReadAsStringAsync();
                var empSales = JsonConvert.DeserializeObject<List<EmployeeSalesPerHourIncrement>>(empSalesJson);
                return empSales;
            }
            else
            {
                return null;
            }
        }

        public async Task<StoreSalePerHour> GetSalesPerHour(string concept, int storeId, DateTime startDate, DateTime endDate)
        {
            var response = await GetAsync($"api/{concept}/teamsales/salesperhour?storeId={storeId}&startDate={startDate.ToString("yyyy-MM-dd")}&endDate={endDate.ToString("yyyy-MM-dd")}");
            if (response.IsSuccessStatusCode)
            {
                var storeSalesJson = await response.Content.ReadAsStringAsync();
                var storeSales = JsonConvert.DeserializeObject<StoreSalePerHour>(storeSalesJson);
                return storeSales;
            }
            else
            {
                return null;
            }
        }
    }
}
