using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GADev.Chat.Api.Configurations;
using GADev.Chat.Api.Models;
using GADev.Chat.Application.Business;
using GADev.Chat.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GADev.Chat.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly IUserBusiness _userBusiness;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IOptions<AppSettings> appSettings, IUserBusiness userBusiness)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _userBusiness = userBusiness;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser(){
            try {
                var _identity = (ClaimsIdentity)User.Identity;
                var userId = int.Parse(_identity.FindFirst("userId").Value);

                var user = await _userBusiness.GetUser(userId);

                return Ok(user);
            }
            catch {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistration userRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(userRegistration);
            }

            var user = new ApplicationUser
            {
                Name = userRegistration.Name,
                UserName = userRegistration.Email,
                Email = userRegistration.Email,
                EmailConfirmed = true
            };

            var existsUser = await _userManager.FindByEmailAsync(userRegistration.Email);

            if (existsUser != null) {
                return Forbid();
            }

            var result = await _userManager.CreateAsync(user, userRegistration.Password);

            if (result.Succeeded){
                var newUser = await _userManager.FindByEmailAsync(userRegistration.Email);

                if (!string.IsNullOrEmpty(userRegistration.Avatar)){
                    await _userBusiness.SaveAvatar(userRegistration.Avatar, newUser.Id);
                }

                await _signInManager.SignInAsync(user, false);
                
                var token = GenerateToken(newUser);

                return Ok(token);
            }

            return BadRequest(userRegistration);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(userLogin);
            }

            var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(userLogin.Email);
                var token = GenerateToken(user);
                return Ok(token);
            }

            return BadRequest(userLogin);
        }

        private object GenerateToken(ApplicationUser user)
        {
            var identityClaims = new ClaimsIdentity(
                new[] {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("userId", user.Id.ToString())
                }
            );

            var creationDate = DateTime.Now;
            var expirationDate = DateTime.UtcNow.AddHours(_appSettings.Expiration);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,
                NotBefore = creationDate,
                Expires = expirationDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return new {
                authenticated = true,
                created = creationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)),
                message = "OK"
            };
        }
    }
}