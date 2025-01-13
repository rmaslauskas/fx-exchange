namespace Exchange.Domain.UnitTests.CommandHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Exchange.Domain.CommandHandlers;
    using Exchange.Domain.Commands;
    using Exchange.Domain.DTOs;
    using Exchange.Domain.Enums;
    using Exchange.Domain.Factories.Contracts;
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;

    public class ExchangeCommandHandlerTests
    {
        private ExchangeCommandHandler _sut;
        private ICurrencyPairFactory _currencyPairFactory;

        [SetUp]
        public void Setup()
        {
            _currencyPairFactory = Substitute.For<ICurrencyPairFactory>();

            _currencyPairFactory.CreateCurrencyPairAsync(Arg.Any<string>())
              .Returns(new CurrencyPairResult { PrimaryCurrency = "EUR", SecondaryCurrency = "DKK", ExchangeRate = 1.7m });

            _sut = new ExchangeCommandHandler(_currencyPairFactory);
        }

        [Test]
        public async Task Handle_ArgsPassedForAmount100_CurrencyPairFactoryCalled()
        {
            var currencyPair = "EUR/DKK";
            var command = new CalculateExchangeCommand(currencyPair, "100");

            var result = await _sut.Handle(command, CancellationToken.None);

            await _currencyPairFactory.Received().CreateCurrencyPairAsync(currencyPair);
        }

        [Test]
        public async Task Handle_ArgsPassedForAmount100_CalculatedAmountReturned()
        {
            var currencyPair = "EUR/DKK";
            var command = new CalculateExchangeCommand(currencyPair, "100");

            var result = await _sut.Handle(command, CancellationToken.None);

            result.Result.Amount.Should().Be(170);
            await _currencyPairFactory.Received().CreateCurrencyPairAsync(Arg.Any<string>());
        }
    }
}