namespace Exchange.Application.Commands
{
    using Exchange.Domain.DTOs;
    using Exchange.Application.Validators;
    using MediatR;

    public class CalculateExchangeCommand : IRequest<ValidateableResponse<ResultDto>>
    {
        public CalculateExchangeCommand(string currencyPair, string amount)
        {
            CurrencyPair = currencyPair;
            Amount = amount;
        }

        public string CurrencyPair { get; }

        public string Amount { get; }
    }
}