namespace WebApi.Controllers
{
    [Route("authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private UserManager<IdentityUser> UserManager;
        private SignInManager<IdentityUser> SignInManager;
        private IConfiguration Configuration;

        public AuthenticationController(UserManager<IdentityUser> UserManager, SignInManager<IdentityUser> SignInManager, IConfiguration Configuration)
        {
            this.UserManager = UserManager;
            this.SignInManager = SignInManager;
            this.Configuration = Configuration;
        }

        [HttpPost("api/login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationCredentials AuthenticationCredentials)
        {
            var User = await UserManager.FindByNameAsync(AuthenticationCredentials.Id);
            if (User != null)
            {
                if ((await SignInManager.PasswordSignInAsync(User, AuthenticationCredentials.Password, false, false)).Succeeded)
                {
                    var securityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = (await SignInManager.CreateUserPrincipalAsync(User)).Identities.First(),
                        Expires = DateTime.Now.AddMinutes(int.Parse(Configuration["BearerTokens:ExpiryMinutes"])),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["BearerTokens:Key"])), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = new JwtSecurityTokenHandler().CreateToken(securityTokenDescriptor);

                    return Ok(new { Succes = true, Token = handler.WriteToken(securityToken) });
                }
            }

            return BadRequest();
        }

        [HttpPost("api/logout")]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return Ok();
        }
    }
}