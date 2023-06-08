using Course.IdentityServer.Dtos;
using Course.IdentityServer.Models;
using MicroService.Shareds.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace Course.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto p)
        {
            var userCreate = new ApplicationUser
            {
                UserName = p.UserName,
                Email = p.Email,
                City = p.City
            };
            var resultCreate = await _userManager.CreateAsync(userCreate, p.Password);
            if (!resultCreate.Succeeded)
            {
                return BadRequest(Response<NoContent>.Fail(resultCreate.Errors.Select(x => x.Description).ToList(), 400));
            }
            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            if (userClaim == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(userClaim.Value.ToString());
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(new UserViewModel { Id = user.Id, UserName = user.UserName, Email = user.Email, City = user.City });
        }
    }
}
