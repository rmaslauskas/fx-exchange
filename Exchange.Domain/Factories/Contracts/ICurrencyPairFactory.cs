namespace Exchange.Domain.Factories.Contracts
{
    using Exchange.Domain.DTOs;
    using System.Threading.Tasks;

    public interface ICurrencyPairFactory
    {
        public Task<CurrencyPairResult> CreateCurrencyPairAsync(string currencyPair);
    }
}