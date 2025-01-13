namespace Exchange.Domain.DTOs
{
    using Exchange.Domain.Enums;

    public class CurrencyPairResult
    {
        public string PrimaryCurrency { get; set; }

        public string SecondaryCurrency { get; set; }

        public decimal ExchangeRate { get; set; }
    }
}