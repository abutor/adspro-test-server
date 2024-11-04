using Adspro.Contract.Providers;
using Adspro.Web.Helpers;

namespace Adspro.Web.Middlewares
{
    public class AuthMiddleware(ITokenProvider _tokenProvider) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = CookiesHelper.GetAuthToken(context.Request);
            if (string.IsNullOrEmpty(token))
            {
                await next(context);
                return;
            }

            var current = context.RequestServices.GetRequiredService<ICurrentUserProvider>();

            var userId = _tokenProvider.GetUserIdFromToken(token);
            if (userId.HasValue)
            {
                var userProvider = context.RequestServices.GetRequiredService<IUserProvider>();
                current.CurrentUser = await userProvider.GetUserById(userId.Value);
            }

            if (!current.IsActive)
            {
                CookiesHelper.ResetAuthToken(context.Response);
            }

            await next(context);
        }
    }
}
