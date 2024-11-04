using Adspro.Contract.Models;
using Adspro.Contract.Providers;
using Adspro.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Adspro.Web.Controllers
{
    [Route("/api/users"), AuthorizedFilter]
    public class UserController(IUserProvider _userProvider) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int page, [FromQuery] int limit)
        {
            var users = await _userProvider.GetUsersAsync(page, limit);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers([FromRoute] Guid id)
        {
            var user = await _userProvider.GetUserById(id);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpGet("me")]
        public IActionResult GetMyUser([FromServices] ICurrentUserProvider current)
        {
            return Ok(current.CurrentUser);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCredentials credentials)
        {
            var result = await _userProvider.CreateUser(credentials);
            return Ok(result);
        }

        [HttpPost("{id}/set-activity")]
        public async Task SetActive([FromRoute] Guid id, [FromBody] bool active)
        {
            await _userProvider.UpdateActivityAsync(id, active);
        }

        [HttpPost("set-activity")]
        public async Task SetBatchActive([FromBody] UserActiveFlagUpdateModel[] updates)
        {
            await _userProvider.UpdateBatchActivityAsync(updates);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            await _userProvider.DeleteUser(id);
            return Ok();
        }
    }
}
