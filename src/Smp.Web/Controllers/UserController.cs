using System;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Repositories;

namespace Smp.Web.Controllers
{
    public class UserController : Controller
    {
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult CreateUser(string username, string password, string email)
        {
            if (string.IsNullOrEmpty(username)) return BadRequest();
            if (string.IsNullOrEmpty(password)) return BadRequest();
            if (string.IsNullOrEmpty(email)) return BadRequest();

            _userRepository.CreateUser(username, password, email);

            return Ok();
        }
    }
}