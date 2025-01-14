namespace Exchange.Domain.UnitTests.Validators
{
    using Exchange.Application.Commands;
    using Exchange.Application.Validators;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Threading;
    using System.Threading.Tasks;

    public class CalculateExchangeCommandValidatorTests
    {
        private CalculateExchangeCommandValidator _calculateExchangeCommandValidator;

        [SetUp]
        public void Setup()
        {
            _calculateExchangeCommandValidator = new CalculateExchangeCommandValidator();
        }

        [Test]
        public async Task ValidateAsync_PassedCommandWithValidArg_NoErrorMessage()
        {
            var command = new CalculateExchangeCommand("EUR/DKK", "100");

            var result = await _calculateExchangeCommandValidator.ValidateAsync(command, CancellationToken.None);

            result.Errors.Should().BeEmpty();
        }

        [Test]
        public async Task ValidateAsync_PassedCommandWithOneArg_ReturnsErrorMessage()
        {
            var command = new CalculateExchangeCommand("EUR/DKK", string.Empty);

            var result = await _calculateExchangeCommandValidator.ValidateAsync(command, CancellationToken.None);

            result.Errors.Should().Contain(e => e.ErrorMessage == "Usage: Exchange <curency pair> <amount to exchange>");
        }

        [Test]
        public async Task ValidateAsync_PassedCommandWithSecondArgNotDecimal_ReturnsErrorMessage()
        {
            var command = new CalculateExchangeCommand("EUR/DKK", "XXX");

            var result = await _calculateExchangeCommandValidator.ValidateAsync(command, CancellationToken.None);

            result.Errors.Should().Contain(e => e.ErrorMessage == "Second argument should be decimal value.");
        }

        [TestCase("AAA")]
        [TestCase("EUR")]
        public async Task ValidateAsync_PassedCommandWithCurrencyPairWithoutMoneyCurency_ReturnsErrorMessage(string currencyPair)
        {
            var command = new CalculateExchangeCommand(currencyPair, "100");

            var result = await _calculateExchangeCommandValidator.ValidateAsync(command, CancellationToken.None);

            result.Errors.Should().Contain(e => e.ErrorMessage == "The currency pair doesn't contain  main and money currencies.");
        }

        [Test]
        public async Task ValidateAsync_PassedCommandWithCurrencyPairWIthSameMainAndMoneyCodes_ReturnsErrorMessage()
        {
            var command = new CalculateExchangeCommand("EUR/EUR", "100");

            var result = await _calculateExchangeCommandValidator.ValidateAsync(command, CancellationToken.None);

            result.Errors.Should().Contain(e => e.ErrorMessage == "Invalid currency pair.");
        }

        [Test]
        public async Task ValidateAsync_PassedCommandWithCurrencyPairWithNotSupportedMainCurrency_ReturnsErrorMessage()
        {
            var command = new CalculateExchangeCommand("AKA/EUR", "100");

            var result = await _calculateExchangeCommandValidator.ValidateAsync(command, CancellationToken.None);

            result.Errors.Should().Contain(e => e.ErrorMessage == "Can't convert main currency for currency pair.");
        }

        [Test]
        public async Task ValidateAsync_PassedCommandWithCurrencyPairWithNotSupportedMoneyCurrency_ReturnsErrorMessage()
        {
            var command = new CalculateExchangeCommand("EUR/AKA", "100");

            var result = await _calculateExchangeCommandValidator.ValidateAsync(command, CancellationToken.None);

            result.Errors.Should().Contain(e => e.ErrorMessage == "Can't convert money currency for currency pair.");
        }
    }
}