namespace Exchange.Domain.UnitTests.CommandHandlers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Exchange.Domain.CommandHandlers;
    using Exchange.Domain.Commands;
    using Exchange.Domain.Contracts;
    using Exchange.Domain.DTOs;
    using Exchange.Domain.Factories;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using NSubstitute;
    using NUnit.Framework;

    public class ExchangeCommandHandlerIntegrationTests
    {
        private ExchangeCommandHandler _sut;

        [SetUp]
        public void Setup()
        {
            var currencyPairs = new CurrencyPairs()
            {
                BaseCode = "DKK",
                Rates = new Dictionary<string, decimal>
                {
                    { "EUR", 0.134419m },
                    { "USD", 0.150804m },
                    { "GBP", 0.117253m },
                    { "SEK", 1.314060m },
                    { "NOK", 1.275510m },
                    { "CHF", 0.146288m },
                    { "JPY", 16.739203m },
                },
            };

            var exchangeRatesService = Substitute.For<IExchangeRatesService>();
            exchangeRatesService.GetCurrencyPairsAsync(Arg.Any<string>()).Returns(currencyPairs);

            _sut = new ExchangeCommandHandler(new CurrencyPairFactory(exchangeRatesService));
        }

        [TestCase("EUR/DKK", 743.94)]
        [TestCase("NOK/DKK", 78.40)]
        [TestCase("USD/DKK", 663.11)]
        [TestCase("CHF/DKK", 683.58)]
        [TestCase("SEK/DKK", 76.10)]
        [TestCase("GBP/DKK", 852.85)]
        [TestCase("JPY/DKK", 5.97)]
        public async Task Handle_ArgsPassedForAmount100_CalculatedAmmountReturned(string currencyPair, decimal result)
        {
            var command = new CalculateExchangeCommand(currencyPair, "100");

            var response = await _sut.Handle(command, CancellationToken.None);

            response.Result.Amount.Should().Be(result);
        }
    }
}