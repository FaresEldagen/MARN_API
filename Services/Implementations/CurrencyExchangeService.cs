using System.Text.Json;
using MARN_API.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace MARN_API.Services.Implementations
{
    /// <summary>
    /// Currency exchange service that fetches live rates from the ExchangeRate-API (open access, no key required).
    /// Rates are cached for 6 hours since the API updates daily.
    /// Falls back to a static EGP/USD rate if the API is unreachable.
    /// </summary>
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CurrencyExchangeService> _logger;

        // Static fallback rates in case the API is unreachable
        private static readonly Dictionary<string, decimal> FallbackRates = new(StringComparer.OrdinalIgnoreCase)
        {
            { "EGP_USD", 0.0189m },  // 1 EGP ≈ 0.019 USD (roughly 1 USD = 52.9 EGP)
        };

        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(6);
        private const string ApiBaseUrl = "https://open.er-api.com/v6/latest";

        public CurrencyExchangeService(
            HttpClient httpClient,
            IMemoryCache cache,
            ILogger<CurrencyExchangeService> logger)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            _cache = cache;
            _logger = logger;
        }

        public async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            fromCurrency = fromCurrency.ToUpperInvariant();
            toCurrency = toCurrency.ToUpperInvariant();

            if (fromCurrency == toCurrency)
                return 1m;

            var cacheKey = $"exchange_rate_{fromCurrency}_{toCurrency}";

            if (_cache.TryGetValue(cacheKey, out decimal cachedRate))
            {
                _logger.LogDebug("Using cached exchange rate for {From}/{To}: {Rate}", fromCurrency, toCurrency, cachedRate);
                return cachedRate;
            }

            try
            {
                var url = $"{ApiBaseUrl}/{fromCurrency}";
                var response = await _httpClient.GetStringAsync(url);
                var json = JsonDocument.Parse(response);

                var root = json.RootElement;
                if (root.GetProperty("result").GetString() != "success")
                {
                    _logger.LogWarning("ExchangeRate API returned non-success result for {From}", fromCurrency);
                    return GetFallbackRate(fromCurrency, toCurrency);
                }

                var rates = root.GetProperty("rates");
                if (!rates.TryGetProperty(toCurrency, out var rateElement))
                {
                    _logger.LogWarning("ExchangeRate API does not have rate for {To} (base: {From})", toCurrency, fromCurrency);
                    return GetFallbackRate(fromCurrency, toCurrency);
                }

                var rate = rateElement.GetDecimal();

                _cache.Set(cacheKey, rate, CacheDuration);
                _logger.LogInformation("Fetched live exchange rate {From}/{To}: {Rate}", fromCurrency, toCurrency, rate);

                return rate;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch live exchange rate for {From}/{To}. Using fallback.", fromCurrency, toCurrency);
                return GetFallbackRate(fromCurrency, toCurrency);
            }
        }

        public async Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            var rate = await GetExchangeRateAsync(fromCurrency, toCurrency);
            return amount * rate;
        }

        private decimal GetFallbackRate(string fromCurrency, string toCurrency)
        {
            var key = $"{fromCurrency}_{toCurrency}";
            if (FallbackRates.TryGetValue(key, out var rate))
            {
                _logger.LogWarning("Using fallback rate for {Key}: {Rate}", key, rate);
                return rate;
            }

            // Try reverse lookup
            var reverseKey = $"{toCurrency}_{fromCurrency}";
            if (FallbackRates.TryGetValue(reverseKey, out var reverseRate) && reverseRate != 0)
            {
                var invertedRate = 1m / reverseRate;
                _logger.LogWarning("Using inverted fallback rate for {Key}: {Rate}", key, invertedRate);
                return invertedRate;
            }

            _logger.LogError("No fallback exchange rate available for {From}/{To}. Using 1:1 ratio.", fromCurrency, toCurrency);
            throw new InvalidOperationException($"No exchange rate available for {fromCurrency}/{toCurrency}");
        }
    }
}
