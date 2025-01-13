namespace Exchange.Domain.UnitTests.Strategies
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Exchange.Domain.Contracts;
    using Exchange.Domain.DTOs;
    using Exchange.Domain.Enums;
    using Exchange.Domain.Factories;
    using Exchange.Domain.Factories.Contracts;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using NSubstitute;
    using NUnit.Framework;

    public class CurrencyPairFactoryTests
    {
        private ICurrencyPairFactory _currencyPairFactory;

        [SetUp]
        public void Setup()
        {
            var currencyPairs = new CurrencyPairs()
            {
                BaseCode = "EUR",
                Rates = new Dictionary<string, decimal>
                {
                    { "DKK", 10.0m },
                    { "CHF", 20.0m },
                },
            };

            var exchangeRatesService = Substitute.For<IExchangeRatesService>();
            exchangeRatesService.GetCurrencyPairsAsync(Arg.Any<string>()).Returns(currencyPairs);

            _currencyPairFactory = new CurrencyPairFactory(exchangeRatesService);
        }

        [Test]
        public async Task CreateCurrencyPair_PassedCurrencyPairAsString_CurrencyPairWithRateReturned()
        {
            var currencyPair = "EUR/DKK";

            var result = await _currencyPairFactory.CreateCurrencyPairAsync(currencyPair);

            result.PrimaryCurrency.Should().Be("EUR");
            result.SecondaryCurrency.Should().Be("DKK");
            result.ExchangeRate.Should().Be(10);
        }

        [Test]
        public async Task CreateCurrencyPair_PassedAnyCurrencyPairAsString_CurrencyPairWithCalculatedRateReturned()
        {
            var currencyPair = "CHF/DKK";

            var result = await _currencyPairFactory.CreateCurrencyPairAsync(currencyPair);

            result.PrimaryCurrency.Should().Be("CHF");
            result.SecondaryCurrency.Should().Be("DKK");
            result.ExchangeRate.Should().Be(0.5m);
        }

        [Test]
        public async Task CreateCurrencyPair_PassedANotherCurrencyPairAsString_CurrencyPairWithCalculatedRateReturned()
        {
            var currencyPair = "DKK/CHF";

            var result = await _currencyPairFactory.CreateCurrencyPairAsync(currencyPair);

            result.PrimaryCurrency.Should().Be("DKK");
            result.SecondaryCurrency.Should().Be("CHF");
            result.ExchangeRate.Should().Be(2);
        }

        [Test]
        public async Task CreateCurrencyPair_PassedCurrencyPairAsString_CurrencyPairWithReversedRateReturned()
        {
            var currencyPair = "DKK/EUR";

            var result = await _currencyPairFactory.CreateCurrencyPairAsync(currencyPair);

            result.PrimaryCurrency.Should().Be("DKK");
            result.SecondaryCurrency.Should().Be("EUR");
            result.ExchangeRate.Should().Be(0.1m);
        }

        [TestCase("EUR/CHF")]
        [TestCase("CHF/EUR")]
        [TestCase("DKK/EUR")]
        [TestCase("EUR/DKK")]
        public async Task CreateCurrencyPair_PassedCorrectCurrencyPairAsString_CurrencyPairReturned(string currencyPair)
        {
            var result = await _currencyPairFactory.CreateCurrencyPairAsync(currencyPair);

            result.Should().NotBeNull();
            result.ExchangeRate.Should().BeGreaterThan(0);
        }
    }
}