using KTOP.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace KTOP.Services
{
    public class UserService
    {
        private static readonly string BaseUrl = DeviceInfo.Platform == DevicePlatform.Android? "http://LOCALIP:HTTPPORT/api/User/" : "http://localhost:HTTPPORT/api/User/";
        public UserService() { }

        public static string AccessToken
        {
            get => Preferences.Get("accesstoken", string.Empty);
            set => Preferences.Set("accesstoken", value);
        }

        public static int UserID
        {
            get => Preferences.Get("userid", 0);
            set => Preferences.Set("userid", value);
        }

        public static async Task<bool> Login(string email, string password)
        {
            var login = new Login()
            {
                Email = email,
                Password = password
            };

            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(BaseUrl + "Login", content);
            if (!response.IsSuccessStatusCode) return false;
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Token>(jsonResult);
            UserService.AccessToken = result.AccessToken;
            UserService.UserID = result.UserId;
            return true;
        }

        public static async Task<bool> Register(string userName, string email, string password)
        {
            var register = new Register()
            {
                UserName = userName,
                Email = email,
                Password = password
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(register);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(BaseUrl + "Register", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<UserModel> GetUserById()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetUserById/{UserService.UserID}");
            return JsonConvert.DeserializeObject<UserModel>(response);
        }

        public static async Task<UserModel> GetOtherUserById(int userId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var response = await httpClient.GetStringAsync(BaseUrl + $"GetUserById/{userId}");
            return JsonConvert.DeserializeObject<UserModel>(response);
        }

        public static async Task<bool> EditUserData(string userName, string email)
        {
            var editUser = new UserEditData()
            {
                UserName = userName,
                Email = email
            };
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var json = JsonConvert.SerializeObject(editUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(BaseUrl + $"EditUserData/{UserService.UserID}", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> ChangePassword(string currentPwd,  string newPwd, string confirmPwd)
        {
            var changePwd = new UserChangePwd()
            {
                ConfirmPassword = confirmPwd,
                CurrentPassword = currentPwd,
                NewPassword = newPwd
            };
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var json = JsonConvert.SerializeObject(changePwd);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(BaseUrl + $"ChangePassword/{UserService.UserID}", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> DeleteUser(string password)
        {
            var deleteUser = new UserDeleteAccount() { Password = password };
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", UserService.AccessToken);
            var json = JsonConvert.SerializeObject(deleteUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(BaseUrl + $"DeleteUser/{UserService.UserID}"),
                Content = content
            };
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
    }
}
