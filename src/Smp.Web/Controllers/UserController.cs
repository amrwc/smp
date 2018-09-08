using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Models.Requests;
using Smp.Web.Repositories;
using Smp.Web.Models;
using Smp.Web.Validators;

namespace Smp.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserValidator _userValidator;

        public UserController(IUserRepository userRepository, IUserValidator userValidator)
        {
            _userRepository = userRepository;
            _userValidator = userValidator;
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserRequest user)
        {
            var validationErrors = _userValidator.ValidateCreateUserRequest(user);
            if (validationErrors.Any()) return BadRequest(validationErrors);

            var newUser = new User(user);

            _userRepository.CreateUser(newUser);

            return Ok();
        }
    }
}