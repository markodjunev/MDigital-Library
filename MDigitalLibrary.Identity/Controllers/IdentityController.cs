namespace MDigitalLibrary.Identity.Controllers
{
    using MDigitalLibrary.Controllers;
    using MDigitalLibrary.Identity.Data.Models;
    using MDigitalLibrary.Identity.Models;
    using MDigitalLibrary.Services.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class IdentityController : ApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IOptions<ApplicationSettings> appSettings;
        private readonly ILogger<IdentityController> logger;
        private readonly ICurrentUserService currentUserService;

        public IdentityController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<ApplicationSettings> appSettings,
            ILogger<IdentityController> logger,
            ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.appSettings = appSettings;
            this.logger = logger;
            this.currentUserService = currentUserService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login(LoginInputModel input)
        {
            var user = this.userManager.Users.FirstOrDefault(r => r.UserName == input.UserName);

            if (user == null)
            {
                return BadRequest(new BadRequestViewModel
                {
                    Message = "Incorrect username or password."
                });
            }

            var result = await this.signInManager.PasswordSignInAsync(user.UserName, input.Password, false, false);

            if (result.Succeeded)
            {
                return Ok(new AuthenticationViewModel
                {
                    Message = "You have successfully logged in.",
                    Token = await GenerateJwtToken(user),
                });
            }

            return BadRequest(new BadRequestViewModel
            {
                Message = "Incorrect username or password."
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Register))]
        public async Task<IActionResult> Register(RegisterInputModel input)
        {
            if (this.userManager.Users.Any(x => x.Email == input.Email))
            {
                return BadRequest(new BadRequestViewModel
                {
                    Message = "This e-mail is already in use. Please try with another one."
                });
            }

            if (this.userManager.Users.Any(x => x.UserName == input.UserName))
            {
                return BadRequest(new BadRequestViewModel
                {
                    Message = "This username is already in use. Please try with another one."
                });
            }

            var user = new ApplicationUser
            {
                UserName = input.UserName,
                Email = input.Email,
            };

            var result = await this.userManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
            {
                var addToRoleResult = await userManager.AddToRoleAsync(user, Constants.UserRoleName);
                if (addToRoleResult.Succeeded)
                {
                    return Ok(Login(new LoginInputModel
                    {
                        UserName = input.UserName,
                        Password = input.Password,
                    }));
                }
            }

            return BadRequest(new BadRequestViewModel
            {
                Message = "Something went wrong."
            });
        }

        [HttpPost]
        [Route(nameof(ChangePassword))]
        public async Task<IActionResult> ChangePassword(ChangePasswordInputModel input)
        {
            var user = this.userManager.Users.FirstOrDefault(x => x.Id == this.currentUserService.UserId);

            this.logger.LogInformation("Hello, id {Name}", user.UserName);

            await Console.Out.WriteLineAsync(   "heeleodr");

            if (user == null) {
                this.logger.LogError("Something went wrong changing password for user with id '{UserId}'", this.currentUserService.UserId);

                return BadRequest();
            }

            var validPassword = await this.userManager.CheckPasswordAsync(user, input.CurrentPassword);

            if (!validPassword)
            {
                this.logger.LogInformation("User with id {UserId} has enteres a wrong password", this.currentUserService.UserId);
                return BadRequest();
            }

            await this.userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = Constants.AdministratorRoleName)]
        [Route(nameof(RegisterModerator))]
        public async Task<IActionResult> RegisterModerator(RegisterInputModel input)
        {
            if (this.userManager.Users.Any(x => x.Email == input.Email))
            {
                return BadRequest(new BadRequestViewModel
                {
                    Message = "This e-mail is already in use. Please try with another one."
                });
            }

            if (this.userManager.Users.Any(x => x.UserName == input.UserName))
            {
                return BadRequest(new BadRequestViewModel
                {
                    Message = "This username is already in use. Please try with another one."
                });
            }

            var user = new ApplicationUser
            {
                UserName = input.UserName,
                Email = input.Email,
            };

            var result = await this.userManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
            {
                var addToRoleResult = await userManager.AddToRoleAsync(user, Constants.ModeratorRoleName);
                if (addToRoleResult.Succeeded)
                {
                    return Ok();
                }
            }

            return BadRequest(new BadRequestViewModel
            {
                Message = "Something went wrong."
            });
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = appSettings.Value.Secret;
            var key = Encoding.ASCII.GetBytes(secret);

            var roles = await this.userManager.GetRolesAsync(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault())//for now we support a single role per user in the table UserRoles
                    }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
