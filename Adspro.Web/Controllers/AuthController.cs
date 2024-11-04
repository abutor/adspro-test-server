using Adspro.Contract.Models;
using Adspro.Contract.Providers;
using Adspro.Web.Filters;
using Adspro.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Adspro.Web.Controllers;

[Route("/api/auth")]
public class AuthController(IAuthProvider _authProvider, ITokenProvider _tokenProvider) : Controller
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCredentialsModel credentials)
    {
        var result = await _authProvider.Login(credentials.Username, credentials.Password);
        if (result?.Active == true)
        {
            CookiesHelper.SetAuthToken(Response, _tokenProvider.GetUserIdToken(result.Id));
            return Ok(result);
        }

        return BadRequest();
    }

    [AuthorizedFilter]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        CookiesHelper.ResetAuthToken(Response);
        return Ok();
    }
}
