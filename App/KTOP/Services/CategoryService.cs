using KTOP.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace KTOP.Services
{
    public class CategoryService
    {
        private static readonly string BaseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://LOCALIP:HTTPPORT/api/Category/" : "http://localhost:HTTPPORT/api/Category/";
        public CategoryService() { }

        public static async Task<List<CategoryModel>> GetAllCategories()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + "GetAllCategories");
            return JsonConvert.DeserializeObject<List<CategoryModel>>(response);
        }

        public static async Task<CategoryModel> GetCategoryById(int categoryId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetCategoryById/{categoryId}");
            return JsonConvert.DeserializeObject<CategoryModel>(response);
        }
    }
}
