using System.Text.Json;

namespace TechMoveGLMS.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private const string API_URL = "https://api.exchangerate-api.com/v4/latest/USD";
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(HttpClient httpClient, ILogger<CurrencyService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            try
            {
                var response = await _httpClient.GetStringAsync(API_URL);
                var exchangeData = JsonSerializer.Deserialize<ExchangeRateResponse>(response);

                if (exchangeData?.Rates != null && exchangeData.Rates.TryGetValue(toCurrency, out var rate))
                {
                    _logger.LogInformation($"Current exchange rate {fromCurrency} to {toCurrency}: {rate}");
                    return rate;
                }

                return 19.50m; // Fallback rate
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching exchange rate");
                return 19.50m; // Default fallback rate
            }
        }

        public async Task<decimal> ConvertCurrencyAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            if (fromCurrency == toCurrency) return amount;

            // For this project, we focus on USD to ZAR conversion
            if (fromCurrency == "USD" && toCurrency == "ZAR")
            {
                var rate = await GetExchangeRateAsync(fromCurrency, toCurrency);
                return amount * rate;
            }

            return amount;
        }
    }

    public class ExchangeRateResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("rates")]
        public Dictionary<string, decimal>? Rates { get; set; }
    }
}
