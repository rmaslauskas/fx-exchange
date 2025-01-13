namespace Exchange.Domain.Validators
{
    using System;
    using System.Globalization;
    using Exchange.Domain.Commands;
    using Exchange.Domain.Enums;
    using FluentValidation;

    public class CalculateExchangeCommandValidator : AbstractValidator<CalculateExchangeCommand>
    {
        public CalculateExchangeCommandValidator()
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;

            When(x => x.Amount != null && x.CurrencyPair != null, () =>
            {
                RuleFor(c => c.Amount)
                   .Must(NumberShouldBeDecimal)
                   .WithMessage(x => "Second argument should be decimal value.");

                RuleFor(c => c.Amount)
                   .Must(ArgumentShouldBeNotEmpty)
                   .WithMessage(x => "Usage: Exchange <curency pair> <amount to exchange>");

                RuleFor(c => c.CurrencyPair)
                    .Must(CurrencyPairShouldContainMainAndMoneyCurrencies)
                    .WithMessage(x => "The currency pair doesn't contain  main and money currencies.");

                RuleFor(c => c.CurrencyPair)
                    .Must(CurrencyPairShouldContainValidMainCurrency)
                    .WithMessage(x => "Can't convert main currency for currency pair.");

                RuleFor(c => c.CurrencyPair)
                    .Must(CurrencyPairShouldContainValidMoneyCurrency)
                    .WithMessage(x => "Can't convert money currency for currency pair.");

                RuleFor(c => c.CurrencyPair)
                    .Must(CurrencyPairShouldContainNotEqualsCodes)
                    .WithMessage(x => "Invalid currency pair.");
            });
        }

        private bool NumberShouldBeDecimal(string ammount)
        {
            return decimal.TryParse(ammount, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal amountDecimal);
        }

        private bool ArgumentShouldBeNotEmpty(string argument)
        {
            return !string.IsNullOrWhiteSpace(argument);
        }

        private bool CurrencyPairShouldContainMainAndMoneyCurrencies(string argument)
        {
            return argument.Split("/").Length == 2;
        }

        private bool CurrencyPairShouldContainValidMainCurrency(string argument)
        {
            var mainCurrencyCode = argument.Split("/")[0];

            return Enum.TryParse<CurrencyCode>(mainCurrencyCode, out var result);
        }

        private bool CurrencyPairShouldContainValidMoneyCurrency(string argument)
        {
            var moneyCurrencyCode = argument.Split("/").Length > 1 ? argument.Split("/")[1] : string.Empty;

            return Enum.TryParse<CurrencyCode>(moneyCurrencyCode, out var result);
        }

        private bool CurrencyPairShouldContainNotEqualsCodes(string argument)
        {
            var primaryCurrencyCode = argument.Split("/").Length > 1 ? argument.Split("/")[0] : string.Empty;
            var secondaryCurrencyCode = argument.Split("/").Length > 1 ? argument.Split("/")[1] : string.Empty;

            return !primaryCurrencyCode.Equals(secondaryCurrencyCode);
        }
    }
}