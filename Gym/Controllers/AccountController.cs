using FluentValidation;
using Gym.Dto;
using Gym.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gym.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidator<RegisterUserDto> _validator;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IValidator<RegisterUserDto> validator)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _validator = validator;
        }
    }
}