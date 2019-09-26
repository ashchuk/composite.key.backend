using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using CompositeKey.Core.Enums;
using CompositeKey.Domain.ViewModels;

namespace CompositeKey.API.Attributes
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ErrorResponseViewModel
                {
                    ErrorCode = ErrorCode.BadRequestParameter.ToString(),
                    Description = "Bad request parameters",
                    Errors = context.ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage))
                });
            }
        }
    }
}
