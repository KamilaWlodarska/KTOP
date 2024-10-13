using KTOP.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace KTOP.Services
{
    public class HomeService
    {
        private static readonly string BaseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://LOCALIP:HTTPPORT/api/Home/" : "http://localhost:HTTPPORT/api/Home/";
        public HomeService() { }

        public static async Task<HomeModel> GetHomeById(int homeId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetHomeById/{homeId}");
            return JsonConvert.DeserializeObject<HomeModel>(response);
        }

        public static async Task<List<HomeModel>> SearchUserHomeByName(string homeName)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"SearchUserHomeByName/{UserService.UserID}/{homeName}");
            return JsonConvert.DeserializeObject<List<HomeModel>>(response);
        }

        public static async Task<List<HomeModel>> GetAllUserHomes()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetAllUserHomes/{UserService.UserID}");
            return JsonConvert.DeserializeObject<List<HomeModel>>(response);
        }

        public static async Task<bool> AddUserHome(HomeOwnerAdd homeOwnerAdd)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var json = JsonConvert.SerializeObject(homeOwnerAdd);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(BaseUrl + $"AddUserHome/{UserService.UserID}", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> EditUserHome(int homeId, HomeModel homeModel)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var json = JsonConvert.SerializeObject(homeModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(BaseUrl + $"EditUserHome/{UserService.UserID}/{homeId}", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> DeleteUserHome(int homeId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.DeleteAsync(BaseUrl + $"DeleteUserHome/{UserService.UserID}/{homeId}");
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<List<ProductModel>> GetAllHomeProducts(int homeId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetAllHomeProducts/{homeId}");
            return JsonConvert.DeserializeObject<List<ProductModel>>(response);
        }

        public static async Task<List<UsersHomeModel>> GetAllHomeUsers(int homeId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetAllHomeUsers/{homeId}");
            return JsonConvert.DeserializeObject<List<UsersHomeModel>>(response);
        }

        public static async Task<bool> AddHomeUser(int homeId, string email)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var json = JsonConvert.SerializeObject(email);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(BaseUrl + $"AddHomeUser/{homeId}", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> DeleteHomeUser(int homeId, int userId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.DeleteAsync(BaseUrl + $"DeleteHomeUser/{homeId}/{userId}/{UserService.UserID}");
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
    }
}
