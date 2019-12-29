using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Services;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IAccountsService _accountsService;

        public AccountsController(IUsersRepository usersRepository, IAccountsService accountsService)
        {
            _usersRepository = usersRepository;
            _accountsService = accountsService;
        }

        [HttpGet("[action]/{email:string}")]
        public async Task<IActionResult> ForgottenPassword(string email)
        {
            var user = await _usersRepository.GetUserByEmail(email);
            if (user == null) return NotFound(new Error("invalid_email", "Email must be linked to an existing user account."));

            await _accountsService.InitiatePasswordReset(user.Id, user.Email);

            return Ok();
        }
    }
}