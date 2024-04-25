using System.Text.Json;

namespace SammysBBQ.Data
{
    public class ApiDataFactory : AbsSingleton<ApiDataFactory>
    {

        private const bool DEV = true;
        private const string BASE_ENDPOINT = DEV ? "http://127.0.0.1:5000" : "";
        private readonly HttpClient Http;

        public ApiDataFactory()
        {
            Http = new HttpClient();
        }

        public async Task<JsonDocument?> Get(List<string>? l = null)
        {
            string url = BASE_ENDPOINT + "/get?";

            if (l != null)
            {
                for (int i = 0; i < l.Count(); i++)
                {
                    string li = l[i];
                    url += $"l{i + 1}={li}&";
                }
            }

            try
            {

                var response = await Http.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                JsonDocument? retval;
                try
                {
                    retval = JsonDocument.Parse(content);
                }
                catch (Exception exc)
                {
                    retval = null;
                }

                return retval;
            }
            catch (Exception exc)
            {
                return null;
            }
        }


    }
}
