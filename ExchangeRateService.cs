using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cSharp_BankSystem
{
    public class ExchangeRateService
    {
        private readonly HttpClient httpClient;

        public ExchangeRateService()
        {
            httpClient = new HttpClient();
        }

        public async Task<ExchangeRateData> GetExchangeRatesAsync(string Base, string exchangeTo)
        {
                    string ExchangeRateApiUrl = $"https://v6.exchangerate-api.com/v6/2d8754c1bf6d68b8bbea954d/pair/{Base}/{exchangeTo}";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(ExchangeRateApiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    ExchangeRateData exchangeRateData = JsonConvert.DeserializeObject<ExchangeRateData>(responseBody);
                    return exchangeRateData;
                }
                else
                {
                    Console.WriteLine($"API request failed with status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Failed to retrieve data from the API: {ex.Message}");
                return null;
            }
        }

    }

    public class ExchangeRateData
    {
        public string base_code { get; set; }
        public string target_code { get; set; }
        public string conversion_rate { get; set; }
    }
}
