namespace Exchange.Domain.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Exchange.Domain.Contracts;
    using Exchange.Domain.DTOs;
    using Exchange.Domain.Enums;
    using Exchange.Domain.Factories.Contracts;

    public class CurrencyPairFactory : ICurrencyPairFactory
    {
        private readonly IExchangeRatesService _exchangeRatesService;

        public CurrencyPairFactory(IExchangeRatesService exchangeRatesService)
        {
            _exchangeRatesService = exchangeRatesService;
        }

        public async Task<CurrencyPairResult?> CreateCurrencyPair(string currencyPair)
        {
            var primaryCurrencyCode = currencyPair.Split("/")[0];
            var secondaryCurrencyCode = currencyPair.Split("/")[1];
            var rate = 0m;

            var currencyPairs = await _exchangeRatesService.GetCurrencyPairsAsync(primaryCurrencyCode);

            if (currencyPairs == null) throw new Exception();

            if (currencyPairs.BaseCode.Equals(primaryCurrencyCode))
            {
                rate = currencyPairs.Rates[secondaryCurrencyCode];
            }
            else if (currencyPairs.BaseCode.Equals(secondaryCurrencyCode))
            {
                rate = 1 / currencyPairs.Rates[primaryCurrencyCode];
            }
            else
            {
                foreach (var key in currencyPairs.Rates.Keys)
                {
                    if (currencyPairs.Rates.Keys.Contains(primaryCurrencyCode) &&
                        currencyPairs.Rates.Keys.Contains(secondaryCurrencyCode))
                    {
                        rate = currencyPairs.Rates[secondaryCurrencyCode]
                           / currencyPairs.Rates[primaryCurrencyCode];
                    }
                }
            }

            return new CurrencyPairResult
            {
                PrimaryCurrency = primaryCurrencyCode,
                SecondaryCurrency = secondaryCurrencyCode,
                ExchangeRate = rate,
            };
        }
    }
}