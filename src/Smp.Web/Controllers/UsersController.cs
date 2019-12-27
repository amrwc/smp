using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Models.Requests;
using Smp.Web.Repositories;
using Smp.Web.Models;
using Smp.Web.Services;
using Smp.Web.Validators;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUserValidator _userValidator;
        private readonly ICryptographyService _cryptographyService;

        public UsersController(IUsersRepository usersRepository, IUserValidator userValidator, ICryptographyService cryptographyService)
        {
            _usersRepository = usersRepository;
            _userValidator = userValidator;
            _cryptographyService = cryptographyService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserRequest user)
        {
            var validationErrors = await _userValidator.ValidateCreateUserRequest(user);
            if (validationErrors.Any()) return BadRequest(validationErrors);

            var newUser = new User(user);

            newUser.Password = _cryptographyService.HashAndSaltPassword(newUser.Password);

            await _usersRepository.CreateUser(newUser);

            return Ok();
        }

        [HttpGet("[action]/{id:Guid}"), Authorize]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _usersRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Password = string.Empty;

            return Ok(user);
        }
    }
}