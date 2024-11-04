namespace Adspro.Web.Helpers
{
    public static class CookiesHelper
    {
        private const string _authTokenKey = "auth";

        public static void SetAuthToken(HttpResponse response, string token)
        {
            response.Cookies.Append(_authTokenKey, token);
        }

        public static string? GetAuthToken(HttpRequest request)
        {
            return request.Cookies[_authTokenKey];
        }

        public static void ResetAuthToken(HttpResponse response)
        {
            response.Cookies.Delete(_authTokenKey);
        }
    }
}
