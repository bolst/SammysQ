using System.Text.Json;

namespace SammysBBQ.Data
{
    public class ApiDataFactory : AbsSingleton<ApiDataFactory>
    {

        private readonly string BASE_ENDPOINT = Environment.GetEnvironmentVariable("DB_HOST_URL") ?? "";
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

        public async Task<bool> Set(string newData, List<string>? l = null)
        {
            string url = BASE_ENDPOINT + "/set?";

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
                var response = await Http.PostAsJsonAsync(url, newData);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync() == "success";
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public async Task<bool> Set(List<MenuItemContent> newData, List<string>? l = null)
        {
            string url = BASE_ENDPOINT + "/set?";

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
                var response = await Http.PostAsJsonAsync(url, newData);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync() == "success";
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public async Task<bool> AddMenu(List<MenuItemContent> menuItems, string menuTitle)
        {
            string url = BASE_ENDPOINT + "/addmenu?title=" + menuTitle;

            try
            {
                var response = await Http.PostAsJsonAsync(url, menuItems);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync() == "success";
            }
            catch (Exception exc) { return false; }
        }


    }
}
