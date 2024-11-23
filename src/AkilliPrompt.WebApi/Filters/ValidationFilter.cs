using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AkilliPrompt.WebApi.Models;
using AkilliPrompt.WebApi.Helpers;
using AkilliPrompt.Domain.ValueObjects;

namespace AkilliPrompt.WebApi.Filters;

public sealed class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var validationErrors = context.ModelState
                .Where(x => x.Value!.Errors.Count > 0)
                .Select(x => new ValidationError(
                    x.Key,
                    x.Value!.Errors.Select(e => e.ErrorMessage).ToList()))
                .ToList();

            var response = ResponseDto<object>.Error(
                MessageHelper.GeneralValidationErrorMessage,
                validationErrors);

            context.Result = new BadRequestObjectResult(response);
            return;
        }

        await next();
    }
}
