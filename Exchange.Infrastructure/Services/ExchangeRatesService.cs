using Exchange.Domain.Contracts;
using Exchange.Domain.DTOs;
using System.Text.Json;

namespace Exchange.Infrastructure.Services
{
    public class ExchangeRatesService: IExchangeRatesService
    {
        private readonly HttpClient _httpClient;

        public ExchangeRatesService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ExchangeAPI");
        }

        public async Task<CurrencyPairs?> GetCurrencyPairsAsync(string primaryCurrency)
        {
            var response = await _httpClient.GetAsync(primaryCurrency);
            if (!response.IsSuccessStatusCode) return null;
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return ConvertToCurrencyPairs(jsonResponse);
        }

        private CurrencyPairs? ConvertToCurrencyPairs(string jsonResponse)
        {
            try
            {
                return JsonSerializer.Deserialize<CurrencyPairs>(jsonResponse);
            }
            catch (Exception)
            {
                return null;
            }     
        }

    }
}
