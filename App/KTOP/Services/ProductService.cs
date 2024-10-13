using KTOP.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace KTOP.Services
{
    public class ProductService
    {
        private static readonly string BaseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://LOCALIP:HTTPPORT/api/Product/" : "http://localhost:HTTPPORT/api/Product/";
        public ProductService() { }
                
        public static async Task<ProductModel> GetProductById(int productId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetProductById/{productId}");
            return JsonConvert.DeserializeObject<ProductModel>(response);
        }

        public static async Task<bool> AddProduct(ProductModel productModel)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var json = JsonConvert.SerializeObject(productModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(BaseUrl + "AddProduct", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> EditProduct(int productId, ProductModel productModel)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var json = JsonConvert.SerializeObject(productModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(BaseUrl + $"EditProduct/{productId}", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> DeleteProduct(int productId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.DeleteAsync(BaseUrl + $"DeleteProduct/{productId}");
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<List<ProductModel>> SearchUserProductByName(string productName)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"SearchUserProductByName/{UserService.UserID}/{productName}");
            return JsonConvert.DeserializeObject<List<ProductModel>>(response);
        }

        public static async Task<List<ProductModel>> GetAllUserProducts()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetAllUserProducts/{UserService.UserID}");
            return JsonConvert.DeserializeObject<List<ProductModel>>(response);
        }

        public static async Task<List<DateTime>> GetAllUserProductsExpirationDates()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetAllUserProductsExpirationDates/{UserService.UserID}");
            return JsonConvert.DeserializeObject<List<DateTime>>(response);
        }

        public static async Task<List<ProductModel>> GetAllUserProductsByExpirationDate(DateTime expireDate)
        {
            var formattedDate = expireDate.ToString("yyyy-MM-dd");
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetAllUserProductsByExpirationDate/{UserService.UserID}/{formattedDate}");
            return JsonConvert.DeserializeObject<List<ProductModel>>(response);
        }

        public static async Task<List<ProductModel>> GetAllUserProductsByHome(int homeId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetAllUserProductsByHome/{UserService.UserID}/{homeId}");
            return JsonConvert.DeserializeObject<List<ProductModel>>(response);
        }

        public static async Task<List<ProductModel>> GetAllUserProductsWithOpenDate()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetAllUserProductsWithOpenDate/{UserService.UserID}");
            return JsonConvert.DeserializeObject<List<ProductModel>>(response);
        }

        public static async Task<List<ProductModel>> GetAllUserProductsByCategory(int categoryId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetAllUserProductsByCategory/{UserService.UserID}/{categoryId}");
            return JsonConvert.DeserializeObject<List<ProductModel>>(response);
        }

        public static async Task<List<ProductModel>> GetUserProductsSortedByPurchaseDateAsc()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetUserProductsSortedByPurchaseDateAsc/{UserService.UserID}");
            return JsonConvert.DeserializeObject<List<ProductModel>>(response);
        }

        public static async Task<List<ProductModel>> GetUserProductsSortedByPurchaseDateDsc()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetUserProductsSortedByPurchaseDateDsc/{UserService.UserID}");
            return JsonConvert.DeserializeObject<List<ProductModel>>(response);
        }

        public static async Task<List<ProductModel>> GetUserProductsSortedByOpenDateAsc()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetUserProductsSortedByOpenDateAsc/{UserService.UserID}");
            return JsonConvert.DeserializeObject<List<ProductModel>>(response);
        }

        public static async Task<List<ProductModel>> GetUserProductsSortedByOpenDateDsc()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetUserProductsSortedByOpenDateDsc/{UserService.UserID}");
            return JsonConvert.DeserializeObject<List<ProductModel>>(response);
        }

        public static async Task<List<ProductModel>> GetUserProductsSortedByExpiryDateAsc()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetUserProductsSortedByExpiryDateAsc/{UserService.UserID}");
            return JsonConvert.DeserializeObject<List<ProductModel>>(response);
        }

        public static async Task<List<ProductModel>> GetUserProductsSortedByExpiryDateDsc()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetUserProductsSortedByExpiryDateDsc/{UserService.UserID}");
            return JsonConvert.DeserializeObject<List<ProductModel>>(response);
        }
    }
}
