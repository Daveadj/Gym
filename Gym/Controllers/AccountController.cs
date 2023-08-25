﻿using FluentValidation;
using Gym.Dto;
using Gym.Models;
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

        public AccountController(UserManager<AppUser> userManager, IValidator<RegisterUserDto> validator)
        {
            _userManager = userManager;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto registerUser)
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
                DateofBirth = registerUser.DateofBirth,
            };

            var result = await _userManager.CreateAsync(newUser, registerUser.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Problem Creating User");
            }
            await _userManager.AddToRoleAsync(newUser, "User");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
               {
                   new Claim(ClaimTypes.Name, newUser.Id.ToString())
               }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new
            {
                Token = tokenHandler.WriteToken(token)
            });
        }
    }
}