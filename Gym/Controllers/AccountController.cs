﻿using FluentValidation;
using Gym.Dto;
using Gym.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gym.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidator<RegisterUserDto> _validator;
        private readonly IConfiguration _configuration;
        private readonly IValidator<ChangePasswordDto> _changePasswordValidator;

        public AccountController(UserManager<AppUser> userManager, IValidator<RegisterUserDto> validator, IConfiguration configuration, IValidator<ChangePasswordDto> changePasswordValidator)
        {
            _userManager = userManager;
            _validator = validator;
            _configuration = configuration;
            _changePasswordValidator = changePasswordValidator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUser)
        {
            var validationResult = await _validator.ValidateAsync(registerUser);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userManager.FindByEmailAsync(registerUser.Email);
            if (user != null)
            {
                return BadRequest("This Email is already in use");
            }

            var newUser = new AppUser
            {
                FirstName = registerUser.FirstName,
                MiddleName = registerUser.MiddleName,
                LastName = registerUser.LastName,
                Email = registerUser.Email,
                UserName = registerUser.Email,
                DateofBirth = registerUser.DateofBirth,
            };

            var result = await _userManager.CreateAsync(newUser, registerUser.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Problem Creating User");
            }
            await _userManager.AddToRoleAsync(newUser, "User");

            var token = GenerateJwtToken(newUser);
            return Ok(new
            {
                Token = token,
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto loginUser)
        {
            var user = await _userManager.FindByEmailAsync(loginUser.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginUser.Password))
            {
                var token = GenerateJwtToken(user);
                return Ok(new
                {
                    Token = token,
                });
            }
            return Unauthorized();
        }

        [HttpPost("LogOut")]
        public IActionResult Logout()
        {
            return Ok(new { message = "logged out successfully. " });
        }

        [HttpPost("InitiateResetPassword")]
        public async Task<IActionResult> InitiateResetPassword(PasswordResetEmailDto useremail)
        {
            var user = await _userManager.FindByEmailAsync(useremail.Email);
            if (user == null)
            {
                return Ok(new { Token = "" }); //TODO: replace this with email sent message
            }
            var passwordReset = await _userManager.GeneratePasswordResetTokenAsync(user);
            return Ok(new { Token = passwordReset });
        }

        [HttpPost("CompleteResetPassword")]
        public async Task<IActionResult> CompleteResetPassword(PasswordResetDto passwordReset)
        {
            var user = await _userManager.FindByEmailAsync(passwordReset.Email);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _userManager.ResetPasswordAsync(user, passwordReset.Token, passwordReset.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok();
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, changePassword.CurrentPassword);
            if (!isPasswordValid)
            {
                return BadRequest();
            }
            var isValidPassword = await _changePasswordValidator.ValidateAsync(changePassword);
            if (!isValidPassword.IsValid)
            {
                return BadRequest(isValidPassword.Errors);
            }
            var changedPassword = await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
            if (!changedPassword.Succeeded)
            {
                return BadRequest();
            }
            return Ok();
        }

        private string GenerateJwtToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWT:Key")!);
            var issuer = _configuration.GetValue<string>("JWT:Issuer");
            var audience = _configuration.GetValue<string>("JWT:Audience");

            var claims = new Claim[]
               {
                   new Claim(ClaimTypes.NameIdentifier, user.Id)
               };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}