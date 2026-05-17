namespace MARN_API.Services.Interfaces
{
    public interface ICurrencyExchangeService
    {
        /// <summary>
        /// Gets the exchange rate to convert from one currency to another.
        /// Returns the rate such that: amount_in_fromCurrency * rate = amount_in_toCurrency
        /// </summary>
        Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency);

        /// <summary>
        /// Converts an amount from one currency to another using the current exchange rate.
        /// </summary>
        Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency);
    }
}
