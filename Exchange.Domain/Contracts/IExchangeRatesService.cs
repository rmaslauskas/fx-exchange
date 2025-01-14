using Exchange.Domain.DTOs;
using System.Threading.Tasks;

namespace Exchange.Domain.Contracts
{
    public interface IExchangeRatesService
    {
        public Task<CurrencyPairs> GetCurrencyPairsAsync(string primaryCurrency);
    }
}
