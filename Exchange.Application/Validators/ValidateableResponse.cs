namespace Exchange.Application.Validators
{
    using System.Collections.Generic;
    using FluentValidation.Results;

    public class ValidateableResponse<TModel> : ValidateableResponseBase
        where TModel : class
    {
        public ValidateableResponse(TModel model, IList<ValidationFailure> validationErrors = null)
            : base(validationErrors)
        {
            Result = model;
        }

        public TModel Result { get; }
    }
}