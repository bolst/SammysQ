using System.Text.Json.Serialization;
using SammysBBQ.Data;

namespace SammysBBQ.Auth
{
    public class User
    {
        [JsonPropertyName("name")]
        public string Username { get; set; }

        public async Task<bool> CheckPassword(string attempt)
        {
            return await AuthApi.Validate(Username, attempt);
            //return await ServiceLayer.GetInstance().CheckUserCredentials(this.Username, attempt);
        }
    }
}