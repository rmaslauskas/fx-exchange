namespace Exchange.Domain.CommandHandlers
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Exchange.Domain.Commands;
    using Exchange.Domain.DTOs;
    using Exchange.Domain.Factories.Contracts;
    using Exchange.Domain.Validators;
    using MediatR;

    public class ExchangeCommandHandler : IRequestHandler<CalculateExchangeCommand, ValidateableResponse<ResultDto>>
    {
        private readonly ICurrencyPairFactory _currencyPairFactory;

        public ExchangeCommandHandler(ICurrencyPairFactory currencyPairFactory)
        {
            _currencyPairFactory = currencyPairFactory;
        }

        public async Task<ValidateableResponse<ResultDto>> Handle(CalculateExchangeCommand request, CancellationToken cancellationToken)
        {
            var amountDecimal = decimal.Parse(request.Amount, CultureInfo.InvariantCulture);
            var currencyPair = await _currencyPairFactory.CreateCurrencyPair(request.CurrencyPair);

            var convertedAmount = decimal.Round(currencyPair.ExchangeRate * amountDecimal, 2, MidpointRounding.ToZero);

            return new ValidateableResponse<ResultDto>(new ResultDto { Amount = convertedAmount });
        }
    }
}