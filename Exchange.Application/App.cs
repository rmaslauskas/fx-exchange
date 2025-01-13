namespace Exchange.Application
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Exchange.Domain.Commands;
    using MediatR;
    using Microsoft.Extensions.CommandLineUtils;

    public class App
    {
        private readonly IMediator _mediator;

        public App(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Run(string[] args)
        {
            var commandLineApplication = new CommandLineApplication(false);
            var currencyPairString = commandLineApplication.Argument("currencyPair", "Curency Pair ");
            var amount = commandLineApplication.Argument("amount", "Ammount for echange");
            commandLineApplication.HelpOption("-? | -h | --help");

            commandLineApplication.OnExecute(
                async () =>
                {
                    var result = await _mediator.Send(new CalculateExchangeCommand(currencyPairString.Value, amount.Value));

                    Console.WriteLine(result.IsValidResponse ?
                        result.Result.Amount.ToString("N2", CultureInfo.InvariantCulture) :
                        $"Exception: {result.Errors.First()}");

                    return 0;
                });

            commandLineApplication.Execute(args);
        }
    }
}