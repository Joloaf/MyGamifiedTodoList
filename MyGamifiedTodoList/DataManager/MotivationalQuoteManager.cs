using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyGamifiedTodoList.DataManager
{
    internal class MotivationalQuoteManager
    {
        public static async Task<List<Models.MotivationalQuote>> GetQuote(string uri)
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri("https://api.api-ninjas.com/v1/quotes");
            client.DefaultRequestHeaders.Add("X-Api-Key", "9WCDLvZ6jg93jBpkd9RBBA==4knB04rkgni0yxE6");

            List<Models.MotivationalQuote> quote = null;

            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                quote = JsonSerializer.Deserialize<List<Models.MotivationalQuote>>(responseString);
            }

            return quote;
        }
    }
}
