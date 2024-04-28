using System.Text;
using System.Text.Json;

namespace SammysBBQ.Auth
{
    public static class AuthApi
    {

        private static readonly string BASE_URL = Environment.GetEnvironmentVariable("DB_HOST_URL") ?? "";

        private static HttpClient httpClient = new HttpClient();

        public static async Task<bool> Validate(string username, string password)
        {
            var content = new Dictionary<string, string>
            {
                {"name", username},
                {"password", password}
            };
            var json = JsonSerializer.Serialize(content);
            var strJson = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync(BASE_URL + "/validate", strJson);
                return await response.Content.ReadAsStringAsync() == "success";
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public static async Task<List<User>?> GetUsers()
        {
            string key = Environment.GetEnvironmentVariable("DB_AUTH_KEY") ?? "";
            var authData = new Dictionary<string, string> { { "key", key } };
            var auth = new Dictionary<string, Dictionary<string, string>> { { "authorization", authData } };
            var json = JsonSerializer.Serialize(auth);
            var strJson = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {

                var response = await httpClient.PostAsync(BASE_URL + "/users", strJson);
                response.EnsureSuccessStatusCode();
                var users = await response.Content.ReadFromJsonAsync<List<string>>();

                List<User> retval = new List<User>();

                foreach (string user in users)
                {
                    retval.Add(new User() { Username = user });
                };
                return retval;
            }
            catch (Exception exc)
            {
                return null;
            }
        }
    }
}