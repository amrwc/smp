using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Models.Requests;
using Smp.Web.Repositories;
using Smp.Web.Models;
using Smp.Web.Services;
using Smp.Web.Validators;
using System.Threading.Tasks;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserValidator _userValidator;
        private readonly ICryptographyService _cryptographyService;

        public UserController(IUserRepository userRepository, IUserValidator userValidator, ICryptographyService cryptographyService)
        {
            _userRepository = userRepository;
            _userValidator = userValidator;
            _cryptographyService = cryptographyService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserRequest user)
        {
            var validationErrors = _userValidator.ValidateCreateUserRequest(user);
            if (validationErrors.Any()) return BadRequest(validationErrors);

            var newUser = new User(user);

            newUser.Password = _cryptographyService.HashAndSaltPassword(newUser.Password);

            await _userRepository.CreateUser(newUser);

            return Ok();
        }

        [HttpGet("[action]/{id:Guid}")]
        public async Task<IActionResult> GetUser([FromRoute]Guid id)
        {
            var user = await _userRepository.GetUser(id);

            return user == null ? (IActionResult) NotFound() : Ok(user);
        }
    }
}