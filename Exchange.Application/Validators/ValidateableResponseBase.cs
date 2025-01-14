namespace Exchange.Application.Validators
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Exchange.Domain.DTOs;
    using FluentValidation.Results;

    public class ValidateableResponseBase
    {
        private readonly IList<string> _errors;

        public ValidateableResponseBase(IList<ValidationFailure>? errors = null)
        {
            _errors = errors != null
                ? errors.Select(
                        error => error.ErrorMessage)
                   .ToList()
                : new List<string>();
        }

        public IReadOnlyCollection<string> Errors => new ReadOnlyCollection<string>(_errors);

        public bool IsValidResponse => !_errors.Any();
    }
}