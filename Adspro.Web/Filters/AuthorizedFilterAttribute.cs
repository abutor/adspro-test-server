using Adspro.Contract.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Adspro.Web.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AuthorizedFilterAttribute : Attribute, IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var provider = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserProvider>();
            if (provider.IsAuthorized && provider.IsAuthorized)
            {
                return next();
            }

            context.Result = new UnauthorizedResult();
            return Task.CompletedTask;
        }
    }
}
