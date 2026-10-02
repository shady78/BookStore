using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookStore.API.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var failures = new List<ValidationFailure>();

            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is null)
                    continue;

                // type CreateAuthorRequest
                // validatorType = IValidator<CreateAuthorRequest>
                var validatorType = typeof(IValidator<>)
                    .MakeGenericType(argument.GetType());

                var validator = context.HttpContext.RequestServices
                    .GetService(validatorType) as IValidator;

                if (validator is null)
                {
                    continue;
                }
                var validationContext = new ValidationContext<object>(argument);
                var validationResult = await validator.ValidateAsync(
                    validationContext,
                    context.HttpContext.RequestAborted);

                failures.AddRange(validationResult.Errors);
            }

            if (failures.Count ==0)
            {
                await next();
                return;
            }
            var errors = failures
                .GroupBy(failure => failure.PropertyName)
                .Select(group => new ValidationError(
                    group.Key,
                    group.Select(failure => failure.ErrorMessage)
                    .Distinct()
                    .ToArray()))
                .ToArray();

            //            [
            //  { "field": "Email", "error": "Email is required." },
            //  { "field": "Email", "error": "Email is invalid." }
            //]

//            [
//  {
//                "field": "Email",
//    "errors": [
//      "Email is required.",
//      "Email is invalid."
//    ]
//  }
//]


            context.Result = new BadRequestObjectResult(
                ApiResponse<object?>.Failure(
                    errors,"Validation failed"));
        }
    }
}
